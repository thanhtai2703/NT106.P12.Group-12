using System;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Client
{
    public partial class Game : Form
    {
        private static Socket clientSocket;
        //Tạo luồng mạng để gửi nhận dữ liệu 
        private static NetworkStream Stream;
        private bool Receiving = true; //Biến receiving để đánh dấu khi nào nhận, không nhận tin nhắn từ server.
        //Lưu giá trị của Xúc xắc, vị trí trên bàn cờ, ID của người chơi, 
        private static int Dice, CurrentPosition, CurrentPlayerId, RedDotsNameSplitter, BlueDotsNameSplitter;
        //Lưu thông tin của người chơi 
        private readonly Player[] Players = new Player[2];
        //Lưu thông tin của các ô trên bàn cờ 
        private readonly Property[] Properties = new Property[40];
        //Chứa hình ảnh của các ô 
        private readonly PictureBox[] Tile;
        private string messagetype=""; //control message
        public static int counter=0;
        private readonly int[] Opportunity = { -100, 100, -150, 150, -200, 200 };
        private System.Windows.Forms.Timer turnTimer;
        //Khởi tạo bộ đếm
        private int timeLeft;
        private int turnTimeLimit = 20; 
        //Nhận dữ liệu trên Server
        private class ReceivedMessage
        {
            public int EndPosition, Balance;
            public readonly int[] PropertiesOwned = new int[40];
        }
        public Game()
        {
            InitializeComponent();
            //this.Hide();
            //Tạo các ô trên bàn cờ và người chơi 
            #region Creating tiles and players
            Tile = new[]
            {
                tile0, tile1, tile2, tile3, tile4, tile5, tile6, tile7, tile8, tile9, tile10,
                tile11, tile12, tile13, tile14, tile15, tile16, tile17, tile18, tile19, tile20,
                tile21, tile22, tile23, tile24, tile25, tile26, tile27, tile28, tile29, tile30,
                tile31, tile32, tile33, tile34, tile35, tile36, tile37, tile38, tile39
            };
            CreateTile("GO", false, "Null", 0, 0);
            CreateTile("Phú Lâm", true, "Purple", 60, 1);
            CreateTile("Khí vận", false, "Opportunity", 0, 2);
            CreateTile("Nhà bè Phú Xuân", true, "Purple", 60, 3);
            CreateTile("Thuế lợi tức", false, "White", 0, 4);
            CreateTile("Bến xe Lục Tỉnh", true, "Station", 200, 5);
            CreateTile("Thị Nghè", true, "Turquoise", 100, 6);
            CreateTile("Cơ hội", false, "Opportunity", 0, 7);
            CreateTile("Tân Định", true, "Turquoise", 100, 8);
            CreateTile("Bến Chương Dương", true, "Turquoise", 120, 9);
            CreateTile("Thăm tù", false, "Null", 0, 10);
            CreateTile("Phan Đình Phùng", true, "Pink", 140, 11);
            CreateTile("Công ty điện lực", true, "Station", 140, 12);
            CreateTile("Trịnh Minh Thế", true, "Pink", 140, 13);
            CreateTile("Lý Thái Tổ", true, "Pink", 160, 14);
            CreateTile("Bến xe Lam Chợ Lớn", true, "Station", 200, 15);
            CreateTile("Đại lộ Hùng Vương", true, "Orange", 180, 16);
            CreateTile("Khí vận", false, "Opportunity", 0, 17);
            CreateTile("Gia Long", true, "Orange", 180, 18);
            CreateTile("Bến Bạch Đằng", true, "Orange", 200, 19);
            CreateTile("Sân bay", false, "Null", 0, 20);
            CreateTile("Đường Công Lý", true, "Red", 220, 21);
            CreateTile("Cơ hội", false, "Opportunity", 0, 22);
            CreateTile("Đại lộ thống nhất", true, "Red", 220, 23);
            CreateTile("Đại lộ Cộng Hòa", true, "Red", 240, 24);
            CreateTile("Bến xe An Đông", true, "Station", 200, 25);
            CreateTile("Đại lộ Hồng Thập Tự", true, "Yellow", 260, 26);
            CreateTile("Đại lộ Hai Bà Trưng", true, "Yellow", 260, 27);
            CreateTile("Công ty thủy cục", true, "Station", 150, 28);
            CreateTile("Xa lộ Biên Hòa", true, "Yellow", 280, 29);
            CreateTile("VÔ TÙ", false, "Null", 0, 30);
            CreateTile("Phan Thanh Giảm", true, "Green", 300, 31);
            CreateTile("Lê Văn Duyệt", true, "Green", 300, 32);
            CreateTile("Khí vận", false, "Opportunity", 0, 33);
            CreateTile("Nguyễn Thái Học", true, "Green", 320, 34);
            CreateTile("Tân Kì Tân Quý", true, "Station", 400, 35);
            CreateTile("Cơ hội", false, "Opportunity", 0, 36);
            CreateTile("Nha Trang", true, "Blue", 350, 37);
            CreateTile("Thuế lương bổng", false, "White", 0, 38);
            CreateTile("Cố Đô Huế", true, "Blue", 400, 39);

            Players[0] = new Player();
            Players[1] = new Player();
            #endregion //Cập nhật giao diện người chơi 
        }

        private void Game_Load(object sender, EventArgs e)
        {
            //Nếu nhiều người chơi 
            if (Gamemodes.Multiplayer)
                try
                {
                    //Hiển thị form kết nối
                    Connection connection = new Connection();
                    connection.ShowDialog();
                    //Nếu chọn Cancel thì hủy kết nối ròi quay về MainMenu chính 
                    if (connection.DialogResult is DialogResult.Cancel)
                    {
                        this.Close();
                        return;
                        //MainMenu mainMenu = new MainMenu();
                        //mainMenu.ShowDialog();
                        ////Stream.Write(
                        //    Encoding.Unicode.GetBytes("Rời"+";" + ConnectionOptions.PlayerName),
                        //    0,
                        //    Encoding.Unicode.GetBytes("Rời" +";"+ ConnectionOptions.PlayerName).Length);
                        //Disconnect();
                    }

                    //Hiển thị thông điệp là đang đợi người chơi thứ 2 nên vô hiệu hóa các nút chơi
                    currentPlayersTurn_textbox.Text = "Chờ đợi người chơi thứ hai...";
                    throwDiceBtn.Enabled = false;
                    buyBtn.Enabled = false;
                    endTurnBtn.Enabled = false;
                    if(Gamemodes.Create) Startbtn.Visible = true;
                    else Startbtn.Visible = false;
                    //Kết nối tới Server
                    try
                    {
                        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        clientSocket.Connect(ConnectionOptions.IP, ConnectionOptions.Port);
                        Stream = new NetworkStream(clientSocket);
                    }
                    catch
                    {
                        MessageBox.Show("Không thể kết nối tới server."
                                        + Environment.NewLine
                                        + "Server không hoạt động");
                        this.InstanceDisconnect();
                    }

                    //Tạo luồng nhận dữ liệu từ Server 
                    Thread receiveThread = new Thread(ReceiveMessage);
                    receiveThread.Start();

                    //SendMessageToServer("Người chơi mới đã vào"+";"+ConnectionOptions.Room);
                    if (Gamemodes.Create) SendMessageToServer("Tạo phòng" + ";" + ConnectionOptions.Room);
                    else SendMessageToServer("Tham gia" + ";" + ConnectionOptions.Room);

                    //Hiển thị Form chọn màu 
                    ColorChoosing colorChoosing = new ColorChoosing();
                    colorChoosing.ShowDialog();
                    //Nếu chọn Cancel thì hủy kết nối ròi quay về MainMenu chính 
                    if (colorChoosing.DialogResult is DialogResult.Cancel)
                    {
                        messagetype = "Thoát";
                        //Disconnect();
                        //return;
                        //this.Close();
                        //return;
                        //MainMenu mainMenu = new MainMenu();
                        //mainMenu.ShowDialog();
                        SendMessageToServer(messagetype+";"+ConnectionOptions.PlayerName);
                        this.InstanceDisconnect();
                       // Disconnect();
                        //this.Close();
                    }
                    //Gửi tên  người chơi đến server
                    //SendMessageToServer("Kết nối"+";"+ConnectionOptions.PlayerName);
                    SendMessageToServer("Kết nối" + ";" + ConnectionOptions.PlayerName +";"+ ConnectionOptions.UserName+";");
                    //Xác định người chơi hiện tại và đánh dấu họ đã kết nối 
                    string[] player = ConnectionOptions.PlayerName.Split(';');
                    switch (player[0])
                    {
                        case "Đỏ":
                            //Players[0].Name = ConnectionOptions.RedUserName;
                            Player1Name.Text = ConnectionOptions.UserName;
                                colorLb.BackColor = Color.Red;
                                CurrentPlayerId = 0;
                           break;
                        case "Xanh":
                            Player2Name.Text = ConnectionOptions.UserName;
                            //Players[1].Name = ConnectionOptions.BlueUserName;
                            colorLb.BackColor = Color.Blue;
                            CurrentPlayerId = 1;
                            break;

                    }
        
                    colorLb.Text = ConnectionOptions.Room;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            
            UpdatePlayersStatusBoxes();
            buyBtn.Enabled = false;
        }
        

        //Tạo ô cờ gồm tên, màu, có thể mua được, giá, vị trí 
        private void CreateTile(string tileName, bool tileBuyable, string tileColor, int tilePrice, int tilePosition)
        {
            //Tạo các đối tượng và gán giá trị 
            Property property = new Property
            {
                Name = tileName,
                Color = tileColor,
                Buyable = tileBuyable,
                Price = tilePrice
            };
            //Gắn ô trên bàn cờ vào vị trí tương ứng trong mảng Properties 
            Properties[tilePosition] = property;
        }
        //Chuyển danh sách tài sản thành 1 chuỗi để hiển thị 
        private string PropertiesToString(int[] propertyList)
        {
            var tempString = "";
            //Chạy qua ds tài sản, sau đó thêm tên, màu vào chuỗi 
            for (var i = 0; i < 40; i++)
                if (propertyList[i] != 0)
                    tempString = tempString + Properties[propertyList[i]].Name + ", " + Properties[propertyList[i]].Color + "\n";
            return tempString;
        }
        //Cập nhật thông tin về người chơi trên giao diện 
        private void UpdatePlayersStatusBoxes()
        {
            if (redPlayerStatusBox_richtextbox.InvokeRequired)
            {
                redPlayerStatusBox_richtextbox.Invoke((MethodInvoker)delegate
                {
                    redPlayerStatusBox_richtextbox.Text =
                "Tiền còn lại: " + Players[0].Balance + "\n"
                + PropertiesToString(Players[0].PropertiesOwned);
                });
                }
            else
            {
                redPlayerStatusBox_richtextbox.Text =
               "Tiền còn lại: " + Players[0].Balance + "\n"
              + PropertiesToString(Players[0].PropertiesOwned);
            }
            if (bluePlayerStatusBox_richtextbox.InvokeRequired)
            {
                bluePlayerStatusBox_richtextbox.Invoke((MethodInvoker)delegate
                {
                    bluePlayerStatusBox_richtextbox.Text =
                         "Tiền còn lại: " + Players[1].Balance + "\n"
                        + PropertiesToString(Players[1].PropertiesOwned);
                });
            }
            else
            {
                bluePlayerStatusBox_richtextbox.Text =
                         "Tiền còn lại: " + Players[1].Balance + "\n"
                        + PropertiesToString(Players[1].PropertiesOwned);
            }    
        }
        //Thay đổi số dư và cập nhật lên giao diện 
        private void ChangeBalance(Player player, int cashChange)
        {
            player.Balance += cashChange;
            UpdatePlayersStatusBoxes();
        }
        //Đưa người chơi vào tù 
        private void InJail()
        {
            //Tăng số lần vào tù
            Players[CurrentPlayerId].Jail += 1;
            //Vô hiệu hóa các nút khi vào tù 

            buyBtn.Enabled = false;
            throwDiceBtn.Enabled = false;
            endTurnBtn.Enabled = true;
            //Thông báo tình trạng người chơi 
            switch (CurrentPlayerId)
            {
                case 0:
                    currentPlayersTurn_textbox.Text =
                        Player1Name.Text + " bạn đang ở tù!\r\nLượt của bạn sẽ bị bỏ qua và tới lượt kế. "; break;
                case 1:
                    currentPlayersTurn_textbox.Text =
                        Player2Name.Text + " Xanh, bạn đang ở tù.!\r\nLượt của bạn sẽ bị bỏ qua và tới lượt kế. ";
                    break;
            }
            //Nếu người chơi đã vào tù 3 lần thì thả người chơi ra 
            if (Players[CurrentPlayerId].Jail != 3) 
                return;
            //Sau khi thả người chơi thì hiển thị thông báo trên dao diện
            Players[CurrentPlayerId].InJail = false;
            Players[CurrentPlayerId].Jail = 0;
            throwDiceBtn.Enabled = true;
            switch (CurrentPlayerId)
            {
                case 0:
                    currentPlayersTurn_textbox.Text =
                        Player1Name.Text + " Đỏ, bạn đã được thả! ";
                    break;
                case 1:
                    currentPlayersTurn_textbox.Text =
                        Player2Name.Text + " Xanh, bạn đã được thả! ";
                    break;
            }
        }
        //Trả về số tiền thuê tại vị trí hiện tại dựa trên giá trị của xúc xắc 
        private int GetRent(int dice)
        {
            //Xác định loại tài sản tại vị trí hiện tại và tính tiền thuê tương ứng 
            switch (Properties[CurrentPosition].Color)
            {
                case "Null":
                    Properties[CurrentPosition].Rent = 0;
                    break;
                case "Station":
                    Properties[CurrentPosition].Rent = dice * 20;
                    break;
                case "White":
                    Properties[CurrentPosition].Rent = 0;
                    break;
                case "Opportunity ":
                    Properties[CurrentPosition].Rent = 0;
                    break;
                case "Purple":
                    Properties[CurrentPosition].Rent = 60;
                    break;
                case "Turquoise":
                    Properties[CurrentPosition].Rent = 120;
                    break;
                case "Pink":
                    Properties[CurrentPosition].Rent = 160;
                    break;
                case "Orange":
                    Properties[CurrentPosition].Rent = 200;
                    break;
                case "Red":
                    Properties[CurrentPosition].Rent = 240;
                    break;
                case "Yellow":
                    Properties[CurrentPosition].Rent = 280;
                    break;
                case "Green":
                    Properties[CurrentPosition].Rent = 320;
                    break;
                case "Blue":
                    Properties[CurrentPosition].Rent = 400;
                    break;
            }
            return Properties[CurrentPosition].Rent;
        }
        //Vẽ hình tròn đại diện cho vị trí người chơi trên bàn cờ 
        private void DrawCircle(int position, int playerId)
        {
            //Lấy tọa độ x, y của các ô trên bàn cờ 
            int x = Tile[position].Location.X, y = Tile[position].Location.Y;
            //Tạo hình tròn đại diện cho người chơi và đặt tọa độ và hình ảnh tương ứng 
            switch (playerId)
            {
                //Người chơi màu đỏ 
                case 0:
                    {
                        var redMarker = new PictureBox
                        {
                            Size = new Size(30, 30),
                            Name = "redMarker" + RedDotsNameSplitter,
                            BackgroundImage = redDot_picturebox.BackgroundImage,
                            BackColor = Color.Transparent,
                            Left = x,
                            Top = y
                        };
                        //Thêm hình tròn vào danh sách các control và đưa lên phía trước 
                        Controls.Add(redMarker);
                        redMarker.BringToFront();
                        //Tăng biến đếm tên của hình tròn đỏ 
                        RedDotsNameSplitter++;
                        break;
                    }
                case 1:
                    {
                        var blueMarker = new PictureBox
                        {
                            Size = new Size(30, 30),
                            Name = "blueMarker" + BlueDotsNameSplitter,
                            BackgroundImage = blueDot_picturebox.BackgroundImage,
                            BackColor = Color.Transparent,
                            Left = x,
                            Top = y
                        };
                        Controls.Add(blueMarker);
                        blueMarker.BringToFront();
                        //Tăng biến đếm tên của hình tròn xanh 
                        BlueDotsNameSplitter++;
                        break;
                    }
            }
        }
        private void UpdateTimeDisplay()
        {
            timeLabel.Text = $"Time left: {timeLeft} seconds"; // Hiển thị thời gian còn lại
        }
        //Nhận các tin nhắn từ server và xử lý
        private void ReceiveMessage()
        {
            //Lặp vô hạn để liên tục nhận tin nhắn từ máy chủ 
            while (Receiving)
                try
                {
                    //Tạo mảng byte để chứa dữ liệu từ máy chủ 
                    byte[] data = new byte[256];
                    //Tạo một StringBuilder để xây dựng chuỗi tù ư dũ liệu nhận được 
                    StringBuilder builder = new StringBuilder();
                    //Đọc dữ liệu từ luồng và thêm vào StringBuilder cho tới khi không còn dữ liệu khả dụng 
                    do
                    {
                        var bytes = Stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (Stream.DataAvailable);
                    //Chuyển StringBuilder thành chuỗi 
                    String message = builder.ToString();
                    //Xử lý các loại tin nhắn từ máy chủ
                    // Tách chuỗi ra để dễ cho việc xử lý từng phòng chơi
                    string[] parts = message.Split(';');
                    //string[] parts = message.Split(new char[] { ' ', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

                    //Nhận được thông điệp máy chủ cả 2 ngươi chơi đều đã kết nối
                    //if (Regex.IsMatch(message, @"Cả\s+2\s+người\s+chơi\s+đã\s+kết\s+nối:\s+\d+") && parts[parts.Length - 1] == ConnectionOptions.Room)
                    switch (parts[0])
                    {
                        case "Cập nhật":
                            if (ConnectionOptions.Room == parts[1])
                            {
                                Player1Name.Invoke((MethodInvoker)delegate
                                {
                                    Player1Name.Text = parts[2];
                                });
                                Player2Name.Invoke((MethodInvoker)delegate
                                {
                                    Player2Name.Text = parts[3];
                                });
                                if (Gamemodes.Create)
                                {
                                    currentPlayersTurn_textbox.Invoke((MethodInvoker)delegate
                                    {
                                        currentPlayersTurn_textbox.Text = "Ấn bắt đầu để tiến hành chơi;";
                                    });
                                }
                                else currentPlayersTurn_textbox.Invoke((MethodInvoker)delegate
                                {
                                    currentPlayersTurn_textbox.Text = "Chờ đợi chủ phòng bắt đầu.";
                                });
                                Players[0].Name = parts[2];
                                Players[1].Name = parts[3];
                                UpdatePlayersStatusBoxes();
                            }
                            break;
                        case "Kết nối":
                            //if (parts[2] == ConnectionOptions.Room)
                            //{
                            //    if (parts[1] == "Đỏ") redPlayerStatusBox_richtextbox.Text = parts[3];
                            //    if (parts[1] == "Xanh") bluePlayerStatusBox_richtextbox.Text = parts[3];
                            //}
                            break;
                        case "Bắt đầu":
                            if (ConnectionOptions.Room == parts[2])
                            {
                                if (CurrentPlayerId == Convert.ToInt32(parts[3]))
                                {
                                    currentPlayersTurn_textbox.Invoke((MethodInvoker)delegate
                                    {
                                        timeLeft = turnTimeLimit;
                                        if (turnTimer == null)
                                        {
                                            turnTimer = new System.Windows.Forms.Timer();
                                            turnTimer.Interval = 1000; // 1 giây (1000ms)
                                            turnTimer.Tick += new EventHandler(timer1_Tick);
                                        }
                                        UpdateTimeDisplay();
                                        turnTimer.Start();
                                        currentPlayersTurn_textbox.Text = "Tung xúc sắc để bắt đầu trò chơi";
                                        throwDiceBtn.Enabled = true;
                                        buyBtn.Enabled = false;
                                        endTurnBtn.Enabled = false;
                                    });
                                }
                                else
                                {
                                    currentPlayersTurn_textbox.Invoke((MethodInvoker)delegate
                                    {
                                        currentPlayersTurn_textbox.Text = Player1Name.Text + " Đỏ đang thực hiện lượt chơi. Chờ...";
                                        Startbtn.Enabled = false;
                                    });
                                }
                            }
                            break;
                        case "Nhắn":
                            if (parts[1] == ConnectionOptions.Room)
                            {
                                string decodedMessage = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(parts[3]));
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    messageRTB.Invoke((MethodInvoker)delegate
                                {
                                            messageRTB.AppendText(parts[2] + ": "+decodedMessage+ Environment.NewLine);
                                });
                                    //}
                                }));
                            }
                            break;
                        case "Rời":
                            if (ConnectionOptions.Room == parts[2])
                            {
                                //SendMessageToServer("Rời"+";"+ConnectionOptions.PlayerName);
                                this.Invoke((MethodInvoker)delegate
                                {
                                    MessageBox.Show("Đối thủ của bạn đã rời", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    //QuitGame();
                                    SendMessageToServer("Rời" + ";" + ConnectionOptions.PlayerName);
                                    this.InstanceDisconnect();
                                    //this.Close();
                                    //this.InstanceDisconnect();
                                    //this.Close();
                                    //MainMenu mainMenu = new MainMenu();
                                    //mainMenu.ShowDialog();
                                });
                            }
                            break;
                        case "Kết quả":
                            //string[] infomation = parts[6].Split('~');   
                            if (parts[2] == ConnectionOptions.Room)
                            {

                                int temp = Convert.ToInt32(parts[3]);
                                if (Convert.ToInt32(parts[3]) == CurrentPlayerId)
                                {
                                    currentPlayersTurn_textbox.Invoke((MethodInvoker)delegate
                                    {
                                        // Cập nhật trạng thái của textbox hiển thị lượt của người chơi hiện tại
                                        //bắt đầu điếm thời gian
                                        timeLeft = turnTimeLimit;
                                        if (turnTimer == null)
                                        {
                                            turnTimer = new System.Windows.Forms.Timer();
                                            turnTimer.Interval = 1000; // 1 giây (1000ms)
                                            turnTimer.Tick += new EventHandler(timer1_Tick);
                                        }
                                        UpdateTimeDisplay();
                                        turnTimer.Start();
                                        currentPlayersTurn_textbox.Text = "Lượt của bạn";
                                        if (Players[CurrentPlayerId].InJail)
                                        {
                                            InJail();
                                        }
                                        else
                                        {
                                            // Kích hoạt nút để ném xúc xắc
                                            throwDiceBtn.Enabled = true;
                                            // Vô hiệu hóa nút mua đất
                                            buyBtn.Enabled = false;
                                            // Vô hiệu hóa nút kết thúc lượt đi
                                            endTurnBtn.Enabled = false;
                                        }
                                    });
                                }

                                // Tạo một đối tượng ReceivedMessage để lưu trữ thông điệp nhận được
                                ReceivedMessage receivedMessage = new ReceivedMessage();

                                // Lấy vị trí kết thúc lượt đi từ tin nhắn
                                String stringPosition = parts[4];
                                receivedMessage.EndPosition = Convert.ToInt32(stringPosition);

                                // Lấy số tiền sau lượt đi từ tin nhắn
                                String stringBalance = parts[5];
                                receivedMessage.Balance = Convert.ToInt32(stringBalance);

                                // Lấy tài sản (đất) hiện có từ tin nhắn
                                String stringPropertiesOwned = parts[6];
                                if (stringPropertiesOwned != null)
                                {
                                    // Lấy mã số của các nhà được sở hữu
                                    int[] tempArrayOfPropertiesOwned = stringPropertiesOwned
                                        .Split(' ')
                                        .Where(x => !string.IsNullOrWhiteSpace(x))
                                        .Select(x => int.Parse(x))
                                        .ToArray();
                                    for (int k = 0; k < tempArrayOfPropertiesOwned.Length; k++)
                                        receivedMessage.PropertiesOwned[k] = tempArrayOfPropertiesOwned[k];
                                }
                                UpdatePlayerStatus(temp, receivedMessage);
                                if (Convert.ToInt32(stringBalance) < 0)
                                {
                                    Win();
                                }
                            }
                            break;
                        case "Thuê":
                            if (parts[2] == ConnectionOptions.Room)
                            {
                                string sumOfRentString = parts[3];
                                int sumOfRent = Convert.ToInt32(sumOfRentString);
                                switch (parts[1])
                                {
                                    case "Đỏ":
                                       // string sumOfRentString = parts[3];
                                        //int sumOfRent = Convert.ToInt32(sumOfRentString);
                                        ChangeBalance(Players[0], -sumOfRent);
                                        ChangeBalance(Players[1], sumOfRent);
                                        MessageBox.Show( Player1Name.Text + "trả tiền thuê nhà cho " + Player2Name.Text + sumOfRent);
                                        break;
                                    case "Xanh":
                                       // string sumOfRentString = parts[3];
                                       // int sumOfRent = Convert.ToInt32(sumOfRentString);
                                        ChangeBalance(Players[1], -sumOfRent);
                                        ChangeBalance(Players[0], sumOfRent);
                                        MessageBox.Show(Player2Name.Text + "trả tiền thuê nhà cho " + Player1Name.Text + sumOfRent);
                                        break;
                                }
                            }
                            break;
                        case "Quân đỏ đã được chọn":
                            {
                                if (ConnectionOptions.Room == parts[1])
                                {
                                   ConnectionOptions.RedUserName = parts[2];
                                    ConnectionOptions.NameRedIsTaken = true;
                                }
                                break;
                            }
                        case "Quân xanh đã được chọn":
                            {
                                if (ConnectionOptions.Room == parts[1])
                                {
                                    ConnectionOptions.BlueUserName = parts[2];
                                    ConnectionOptions.NameBlueIsTaken = true;
                                }
                                break;
                            }
                        case "Phòng đã đủ người chơi":
                            if(ConnectionOptions.Room == parts[1])
                            {
                                ConnectionOptions.RedUserName = parts[2];
                                ConnectionOptions.BlueUserName = parts[3];
                                ConnectionOptions.NameBlueIsTaken = true;
                                ConnectionOptions.NameRedIsTaken = true;
                            }    
                            break;
                        case "Không tìm thấy phòng":
                            if(ConnectionOptions.Room == parts[1])
                            {
                                ConnectionOptions.isJoined = false;
                            }   
                         break;
                        case "Phòng đã tồn tại":
                            if(ConnectionOptions.Room == parts[1])
                            {
                                ConnectionOptions.isCreated = true;
                            }
                            break;
                    }

                    
                }
                catch(Exception e) 
                {
                        MessageBox.Show(e.Message);
                    this.InstanceDisconnect();
                }
        }
        //Hàm được gọi khi người chơi thua cuộc

        private void Lose()
        {
            if (MessageBox.Show("Bạn đã thua! Chúc may mắn lần sau!", "Thông báo", MessageBoxButtons.OK) == DialogResult.OK)
            {
                SendMessageToServer("Thua" +";"+ ConnectionOptions.UserName + ";" + ConnectionOptions.Room);
                this.Close();
                this.InstanceDisconnect();
                //MainMenu mainMenu = new MainMenu();
                //mainMenu.ShowDialog();
            }

        }
        private void Win()
        {
            if (MessageBox.Show("Bạn đã thắng! Congratulations!", "Thông báo", MessageBoxButtons.OK) == DialogResult.OK)
            {
                SendMessageToServer("Thắng" +";"+ConnectionOptions.UserName + ";"+ConnectionOptions.Room);
                this.InstanceDisconnect();
                this.Close();
                //this.InstanceDisconnect();
                //this.Close();
                //MainMenu mainMenu = new MainMenu();
                //mainMenu.ShowDialog();
            }

        }
        private void UpdatePlayerStatus(int playerId, ReceivedMessage receivedMessage)
        {
            int temp = CurrentPlayerId;
            CurrentPlayerId = CurrentPlayerId is 0 ? 1 : 0;
            // Di chuyển biểu tượng của người chơi đến vị trí kết thúc lượt đi
            Invoke((MethodInvoker)delegate
            {
                MoveIcon(receivedMessage.EndPosition);
            });

            // Cập nhật vị trí và số dư của người chơi
            Players[CurrentPlayerId].Position = receivedMessage.EndPosition;
            Players[CurrentPlayerId].Balance = receivedMessage.Balance;

            // Cập nhật danh sách tài sản được sở hữu của người chơi
            int i = 0;
            foreach (var item in receivedMessage.PropertiesOwned)
            {
                Players[CurrentPlayerId].PropertiesOwned[i] = item;
                i++;
            }

            // Vẽ các biểu tượng tài sản mà người chơi đang sở hữu
            foreach (var item in Players[CurrentPlayerId].PropertiesOwned)
            {
                if (item != 0)
                {
                    Properties[item].Owned = true;
                    Players[CurrentPlayerId].NumberOfPropertiesOwned++;
                    currentPlayersTurn_textbox.Invoke((MethodInvoker)delegate
                    {
                        DrawCircle(item, CurrentPlayerId);
                    });
                }
            }
            CurrentPlayerId = temp;

            // Cập nhật hộp thông tin trạng thái của các người chơi
            UpdatePlayersStatusBoxes();
        }
        private void StopReceiving()
        {
            Receiving = false;
        }
        public void InstanceDisconnect()
        {
            StopReceiving();  // Dừng luồng nhận tin nhắn
            Disconnect();  // Gọi phương thức static Disconnect
        }
        //Phương thức ngắt kết nối và thoát ứng dụng 
        private static void Disconnect()
        {
            try
            {


                if (Stream != null)
                {
                    Stream.Close();   // Đóng luồng
                    Stream.Dispose(); // Giải phóng tài nguyên nếu cần
                    Stream = null;    // Đặt về null để tránh việc tái sử dụng
                }
                if (clientSocket != null)
                {
                    if (clientSocket.Connected) // Kiểm tra nếu socket vẫn đang mở
                    {
                        clientSocket.Shutdown(SocketShutdown.Both); // Đóng cả gửi và nhận
                    }

                    clientSocket.Close();    // Đóng socket
                    clientSocket.Dispose();  // Giải phóng tài nguyên
                    clientSocket = null;     // Đặt về null để tránh việc tái sử dụng
                }
            }
            catch (Exception ex)
            {
                // Bắt bất kỳ lỗi nào khác
                Console.WriteLine("Lỗi không xác định: " + ex.Message);
            }
            finally
            {
                Environment.Exit(0); //đóng toàn bộ ứng dụng
            }

        }

        //Phương thức di chiển biểu tượng của người chơi
        private void MoveIcon(int position)
        {
            int x, y;
            switch (CurrentPlayerId)
            {
                //Lấy tọa độ mới cho biểu tượng màu đỏ/ xanh và di chuyển đến tọa độ mới 
                case 0:
                    x = Tile[position].Location.X;
                    y = Tile[position].Location.Y;
                    redPawnIcon.Location = new Point(x, y);
                    break;
                case 1:
                    x = Tile[position].Location.X;
                    y = Tile[position].Location.Y;
                    bluePawnIcon.Location = new Point(x, y);
                    break;
            }
        }

        private void sendBt_Click(object sender, EventArgs e)
        {
            messagetype = "Nhắn";
            string message = messageTb.Text.Trim(); // loại bỏ khoảng trắng
            if (string.IsNullOrEmpty(message))
                return;
            string ecoding = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message));
            SendMessageToServer(messagetype+";"+ConnectionOptions.Room+";"+ConnectionOptions.UserName+";"+ ecoding);
            messageTb.Text = "";
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            //messagetype = "Rời";
            //if (Gamemodes.Multiplayer)
            //SendMessageToServer(messagetype +";"+ConnectionOptions.PlayerName+";");
        }

        //Animation di chuyển vị trí
        private async Task<int> MoveTileByTile(int from, int to)
        {
            // Nếu vị trí đích nhỏ hơn 40 (nằm trong phạm vi của bảng), di chuyển từ ô hiện tại đến ô đích
            if (to < 40)
            {
                for (var i = from; i <= to; i++)
                {
                    await Task.Delay(150);
                    MoveIcon(i);
                }
            }
            else
            {
                // Nếu vị trí đích lớn hơn hoặc bằng 40, di chuyển từ ô hiện tại đến ô cuối cùng (39),
                // sau đó di chuyển từ ô đầu tiên (0) đến ô đích với phần dư của vị trí đích sau khi chia cho 40
                for (var i = from; i <= 39; i++)
                {
                    await Task.Delay(150);
                    MoveIcon(i);
                }
                for (var i = 0; i <= to - 40; i++)
                {
                    await Task.Delay(150);
                    MoveIcon(i);
                }
            }
            return 1;
        }

        public void Startbtn_Click(object sender, EventArgs e)
        {
            messagetype = "Bắt đầu";
            SendMessageToServer(messagetype + ";" + ConnectionOptions.PlayerName + ";" + 0);
            Startbtn.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLeft--;

            // Cập nhật giao diện
            UpdateTimeDisplay();

            // Nếu hết thời gian, chuyển lượt
            if (timeLeft <= 0)
            {
                turnTimer.Stop(); // Dừng bộ đếm
                EndTurn(); // Kết thúc lượt của người chơi hiện tại
            }
        }
        private void EndTurn()
        {
            turnTimer.Stop();
            messagetype = "Kết quả";
            if (Gamemodes.Multiplayer)
            {
                currentPositionInfo_richtextbox.Text = string.Empty;
                string turnLogString = string.Empty;
                switch (CurrentPlayerId)
                {
                    case 0:
                        turnLogString = messagetype + ";" + ConnectionOptions.PlayerName + ";" + "1" + ";"; //" Kết quả lượt đi của Đỏ"
                        break;
                    case 1:
                        turnLogString = messagetype + ";" + ConnectionOptions.PlayerName + ";" + "0" + ";";
                        break;
                }
                turnLogString = turnLogString
                    + Players[CurrentPlayerId].Position + ";"
                    + Players[CurrentPlayerId].Balance + ";";
                foreach (var item in Players[CurrentPlayerId].PropertiesOwned)
                    if (item != 0)
                    {
                        turnLogString += item;
                        turnLogString += ' ';
                    }
                if (CurrentPlayerId is 0)
                {
                    currentPlayersTurn_textbox.Text = Player2Name.Text + " Xanh đang thực hiện lượt chơi. Chờ...";
                    SendMessageToServer(turnLogString);
                }
                else
                {
                    currentPlayersTurn_textbox.Text = Player1Name.Text + " Đỏ đang thực hiện lượt chơi. Chờ...";
                    SendMessageToServer(turnLogString);
                }


                if (Players[CurrentPlayerId].Balance < 0)
                    Lose();
                else
                {
                    throwDiceBtn.Enabled = false;
                    buyBtn.Enabled = false;
                    endTurnBtn.Enabled = false;
                }
            }
        }
        //Xử lý khi nút ném xúc xắc 
        private void ThrowDiceBtn_Click(object sender, EventArgs e)
        {
            //Hiển thị thông tin của người chơi hiện tại 
            switch (CurrentPlayerId)
            {
                case 0:
                    currentPlayersTurn_textbox.Text = "Lượt của người chơi " + Player1Name.Text;
                    break;
                case 1:
                    currentPlayersTurn_textbox.Text = "Lượt của người chơi " + Player2Name.Text;
                    break;
            }
            //Tạo các biến để theo dõi việc đi qua các ô đặt biệt 
            bool visitedJailExploration = false
                , visitedTaxTile = false
                , visitedGo = false
                , visitedFreeParking = false
                , goingToJail = false
                , landedOpportunity = false;
            int OppResult = new int();
            //Cho phép người chơi mua đất
            buyBtn.Enabled = true;
            endTurnBtn.Enabled = true;

            //Cập nhật trạng thái của người chơi 
            UpdatePlayersStatusBoxes();

            //Ném xúc sắc 
            Random rand = new Random();
            int firstDice = 5;//rand.Next(1, 7);
            int secondDice = 5;//rand.Next(1, 7);
            Dice = firstDice + secondDice;
            //Hiển thị kết quả xức sắc 
            whatIsOnDices_textbox.Text = "Kết quả tung: " + firstDice + " và " + secondDice + ". Tổng: " + Dice + ". ";
            //vô hiệu hóa sau khi ném 
            throwDiceBtn.Enabled = false;
            //Lưu vị trí trước và sau khi ném
            int positionBeforeDicing = Players[CurrentPlayerId].Position;
            CurrentPosition = Players[CurrentPlayerId].Position + Dice;
            int positionAfterDicing = Players[CurrentPlayerId].Position + Dice;
            //Xử lý người chơi ở trong tù 
            if (Players[CurrentPlayerId].InJail) 
                InJail();

            //Tới các ô chức năng ở 4 góc
            switch (CurrentPosition)
            {
                case 0:
                    buyBtn.Enabled = false;
                    visitedGo = true;
                    break;
                case 10 when Players[CurrentPlayerId].InJail is false:
                    buyBtn.Enabled = false;
                    visitedJailExploration = true;
                    break;
                case 20:
                    buyBtn.Enabled = false;
                    visitedFreeParking = true;
                    break;
                case 30:
                    CurrentPosition = 10;
                    Players[CurrentPlayerId].InJail = true;
                    InJail();
                    goingToJail = true;
                    break;
            }
            //Đi qua ô bắt đầu
            if (CurrentPosition >= 40)
            {
                ChangeBalance(Players[CurrentPlayerId], 200);
                Players[CurrentPlayerId].Position = CurrentPosition - 40;
                CurrentPosition = Players[CurrentPlayerId].Position;
            }
            if (Properties[CurrentPosition].Color is "White")
            {   //Vào ô thuế
                ChangeBalance(Players[CurrentPlayerId], -200);
                buyBtn.Enabled = false;
                visitedTaxTile = true;
            } else if (Properties[CurrentPosition].Color is "Opportunity")
            {   //Vào cơ hội, khí vận
                Random random = new Random();
                int randNum = random.Next(0, Opportunity.Length);
                OppResult = Opportunity[randNum];
                ChangeBalance(Players[CurrentPlayerId], OppResult);
                landedOpportunity = true;
                buyBtn.Enabled = false;
            }            

            Players[CurrentPlayerId].Position = CurrentPosition;
            //Di chuyển tới tù
            switch (goingToJail)
            {
                case true:
                    MoveIcon(10);
                    break;
                case false:
                    _ = MoveTileByTile(positionBeforeDicing, positionAfterDicing);
                    break;
            }

            currentPositionInfo_richtextbox.Text = "Vị trí " + CurrentPosition;
            currentPositionInfo_richtextbox.AppendText("\r\n" + Properties[CurrentPosition].Name);
            currentPositionInfo_richtextbox.AppendText("\r\n" + "Giá " + Properties[CurrentPosition].Price);

            if (visitedJailExploration) 
                currentPositionInfo_richtextbox.AppendText("\r\n" + "Bạn đang thăm tù. ");

            if (visitedTaxTile) 
                currentPositionInfo_richtextbox.AppendText("\r\n" + "Bạn đã nộp thuế");

            if (visitedGo) 
                currentPositionInfo_richtextbox.AppendText("\r\n" + "Nhận 200 sau khi đi qua ô \"GO\". ");

            if (visitedFreeParking) 
                currentPositionInfo_richtextbox.AppendText("\r\n" + "Thư giãn nào...");

            if (landedOpportunity)
                if (CurrentPosition == 2 || CurrentPosition == 17 || CurrentPosition == 33)
                    currentPositionInfo_richtextbox.AppendText("\r\n" + "Bạn nhận được " + Convert.ToString(OppResult) + " tại ô \"Khí vận\".");
                else
                    currentPositionInfo_richtextbox.AppendText("\r\n" + "Bạn nhận được " + Convert.ToString(OppResult) + " tại ô \"Cơ hội\".");

            if (goingToJail)
            {
                currentPositionInfo_richtextbox.AppendText("\r\n" + "Bạn đang ở tù.");
                switch (CurrentPlayerId)
                {
                    case 0:
                        currentPlayersTurn_textbox.Text = "Đỏ, bạn đang ở tù!\r\nLượt của bạn sẽ bị bỏ qua và tới lượt kế. ";
                        break;
                    case 1:
                        currentPlayersTurn_textbox.Text = "Xanh, bạn đang ở tù.!\r\nLượt của bạn sẽ bị bỏ qua và tới lượt kế. ";
                        break;
                }
            }

            currentPositionInfo_richtextbox.ScrollToCaret();

            //Nếu đất này chưa được mua hoặc bản thân đang sở hữu thì kết thúc hàm
            if (Players[CurrentPlayerId].PropertiesOwned[CurrentPosition] == CurrentPosition || !Properties[CurrentPosition].Owned) 
                return;
            //Nếu đất của đối phương thì sẽ thực hiện trả tiền cho đối phương
            buyBtn.Enabled = false;
            switch (CurrentPlayerId)
            {
                case 0:
                    ChangeBalance(Players[0], -GetRent(Dice));
                    ChangeBalance(Players[1], GetRent(Dice));
                    if (Gamemodes.Multiplayer)
                    {
                        string rentMessage = "Thuê"+";"+ConnectionOptions.PlayerName + GetRent(Dice);
                        MessageBox.Show(Player1Name.Text +" trả tiền thuê nhà cho " + Player2Name.Text +":" + GetRent(Dice));
                        SendMessageToServer(rentMessage);
                    }
                    break;
                case 1:
                    ChangeBalance(Players[1], -GetRent(Dice));
                    ChangeBalance(Players[0], GetRent(Dice));
                    if (Gamemodes.Multiplayer)
                    {
                        string rentMessage = "Thuê" + ";" + ConnectionOptions.PlayerName + GetRent(Dice);
                        MessageBox.Show(Player2Name.Text + " trả tiền thuê nhà cho " + Player1Name.Text + ":" + GetRent(Dice));
                        SendMessageToServer(rentMessage);
                    }
                    break;
            }
            switch (CurrentPlayerId)
            {
                case 0:
                    currentPlayersTurn_textbox.Text = Player1Name.Text +" bạn vừa vào đất của người chơi khác và phải đóng tiền ";
                    break;
                case 1:
                    currentPlayersTurn_textbox.Text = Player2Name.Text +" bạn vừa vào đất của người chơi khác và phải đóng tiền ";
                    break;
            }
            if (CurrentPosition is 5 || CurrentPosition is 15 || CurrentPosition is 25 || CurrentPosition is 35) 
                currentPlayersTurn_textbox.Text += Dice * 20;
            else currentPlayersTurn_textbox.Text += Properties[CurrentPosition].Rent;
        }

        private void BuyBtn_Click(object sender, EventArgs e)
        {
            if (Properties[CurrentPosition].Buyable && Properties[CurrentPosition].Owned is false)
                if (Players[CurrentPlayerId].Balance >= Properties[CurrentPosition].Price)
                {
                    ChangeBalance(Players[CurrentPlayerId], -Properties[CurrentPosition].Price);
                    //Lấy vị trí nhà mới
                    //Players[CurrentPlayerId].PropertiesOwned[Players[CurrentPlayerId].NumberOfPropertiesOwned] = CurrentPosition;
                    Players[CurrentPlayerId].PropertiesOwned[CurrentPosition] = CurrentPosition;

                    Properties[CurrentPosition].Owned = true;
                    Players[CurrentPlayerId].NumberOfPropertiesOwned++;
                    UpdatePlayersStatusBoxes();
                    buyBtn.Enabled = false;
                    DrawCircle(CurrentPosition, CurrentPlayerId);
                }
                else 
                    currentPlayersTurn_textbox.Text = "Bạn không đủ tiền";
            else 
                currentPlayersTurn_textbox.Text = "Bạn không thể thực hiện hành động đó";
        }
        private void QuitGame()
        {
            if (MessageBox.Show("Bạn có muốn thoát", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Gamemodes.Multiplayer)
                    SendMessageToServer("Rời" + ";" + ConnectionOptions.PlayerName);
                this.InstanceDisconnect();
                this.Close();

                //MainMenu mainMenu = new MainMenu();
                //mainMenu.ShowDialog();
            }
        }
        private void QuitGameBtn_Click(object sender, EventArgs e)
        {
            QuitGame();
        }

        private void EndTurnBtn_Click(object sender, EventArgs e)
        {
            //turnTimer.Stop();
            messagetype = "Kết quả";
            if (Gamemodes.Multiplayer)
            {
                turnTimer.Stop();
                currentPositionInfo_richtextbox.Text = string.Empty;
                string turnLogString = string.Empty;
                switch (CurrentPlayerId)
                {
                    case 0:
                        turnLogString = messagetype +";"+ ConnectionOptions.PlayerName + ";"+ "1"+";"; //" Kết quả lượt đi của Đỏ"
                        break;
                    case 1:
                        turnLogString = messagetype + ";" + ConnectionOptions.PlayerName + ";" + "0"+";";
                        break;
                }
                turnLogString = turnLogString 
                    + Players[CurrentPlayerId].Position + ";"
                    + Players[CurrentPlayerId].Balance + ";";
                foreach (var item in Players[CurrentPlayerId].PropertiesOwned)
                    if (item != 0)
                    {
                        turnLogString += item;
                        turnLogString += ' ';
                    }
                if (CurrentPlayerId is 0)
                {
                    currentPlayersTurn_textbox.Text = Player2Name.Text + " đang thực hiện lượt chơi. Chờ...";
                    SendMessageToServer(turnLogString);
                }
                else {
                    currentPlayersTurn_textbox.Text = Player1Name.Text + " đang thực hiện lượt chơi. Chờ...";
                    SendMessageToServer(turnLogString);
                }


                if (Players[CurrentPlayerId].Balance < 0)
                    Lose();
                else
                {
                    throwDiceBtn.Enabled = false;
                    buyBtn.Enabled = false;
                    endTurnBtn.Enabled = false;
                }
            }
            if (Gamemodes.Singleplayer)
            {
                if(Players[CurrentPlayerId].Balance<0 )
                {
                    switch (CurrentPlayerId)
                    {
                        case 0:
                            if (MessageBox.Show("Xanh đã thắng!", "Message", MessageBoxButtons.OK) is DialogResult.OK) Application.Exit();
                            break;
                        case 1:
                            if (MessageBox.Show("Đỏ đã thắng!", "Message", MessageBoxButtons.OK) is DialogResult.OK) Application.Exit();
                            break;
                    }
                }    
                CurrentPlayerId = CurrentPlayerId is 0 ? 1 : 0;
                switch (CurrentPlayerId)
                {
                    case 0:
                        currentPlayersTurn_textbox.Text = "Lượt của người chơi Đỏ. ";
                        break;
                    case 1:
                        currentPlayersTurn_textbox.Text = "Lượt của người chơi Xanh. ";
                        break;
                }
                throwDiceBtn.Enabled = true;
                buyBtn.Enabled = false;
                if (Players[CurrentPlayerId].InJail)
                {
                    CurrentPosition = 10;
                    MoveIcon(CurrentPosition);
                    Players[CurrentPlayerId].Position = CurrentPosition;
                }
                currentPositionInfo_richtextbox.Text = string.Empty;
            }
        }
        public static void SendMessageToServer(string message)
        {
            try
            {
                // chuyển chuỗi thành các byte chuyển đi
                byte[] data = Encoding.Unicode.GetBytes(message);

                // Gửi chuỗi thông qua stream
                Stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                // hiển thị màn hình là bị lỗi
                MessageBox.Show("Lỗi khi gửi thông báo tới Server: " + ex.Message);
            }
        }
    }
}