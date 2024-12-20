﻿using System;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
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
        private  Socket clientSocket;
        private NetworkStream Stream;
        private string messagetype = ""; //Lệnh điều khiển
        private bool Receiving = true; //Biến receiving để đánh dấu khi nào bắt đầu nhận, dừng nhận tin nhắn từ server. 
        private int Dice, CurrentPosition, CurrentPlayerId, RedDotsNameSplitter, BlueDotsNameSplitter; //giá trị xúc xắc, vị trí hiện tại, ID , số nhà đỏ, số nhà xanh
        private  bool isTurn;//xác định xem có phải lượt chơi của mình không để bỏ qua khi hết thời gian. 
        private readonly Player[] Players = new Player[2]; //2 người chơi
        private readonly Property[] Properties = new Property[40];//Lưu thông tin của các ô trên bàn cờ 
        private readonly PictureBox[] Tile;//Chứa hình ảnh của các ô 
        public  int counter = 0; //đếm số lượt đã pass
        private readonly int[] Opportunity = { -100, 100, -150, 150, -200, 200 };
        private System.Windows.Forms.Timer turnTimer;
        //Khởi tạo bộ đếm
        private int timeLeft;
        private int turnTimeLimit = 20; 
        //lưu trữ thông tin 1 người chơi
        private class Playerinfomation{
            public int EndPosition, Balance;
            public readonly int[] PropertiesOwned = new int[40];
        }
        public Game()
        {
            InitializeComponent();
            #region Tạo các ô trên bàn cờ và người chơi 
            Tile = new[]
            {
                tile0, tile1, tile2, tile3, tile4, tile5, tile6, tile7, tile8, tile9, tile10,
                tile11, tile12, tile13, tile14, tile15, tile16, tile17, tile18, tile19, tile20,
                tile21, tile22, tile23, tile24, tile25, tile26, tile27, tile28, tile29, tile30,
                tile31, tile32, tile33, tile34, tile35, tile36, tile37, tile38, tile39
            };
            CreateTile("GO", false, "Null", 0, 0,0);
            CreateTile("Phú Lâm", true, "Purple", 60, 1,60);
            CreateTile("Khí vận", false, "Opportunity", 0, 2,0);
            CreateTile("Nhà bè Phú Xuân", true, "Purple", 60, 3,60);
            CreateTile("Thuế lợi tức", false, "White", 0, 4,0);
            CreateTile("Bến xe Lục Tỉnh", true, "Station", 200, 5,100);
            CreateTile("Thị Nghè", true, "Turquoise", 100, 6,120);
            CreateTile("Cơ hội", false, "Opportunity", 0, 7,0);
            CreateTile("Tân Định", true, "Turquoise", 100, 8,120);
            CreateTile("Bến Chương Dương", true, "Turquoise", 120, 9,120);
            CreateTile("Thăm tù", false, "Null", 0, 10,0);
            CreateTile("Phan Đình Phùng", true, "Pink", 140, 11,160);
            CreateTile("Công ty điện lực", true, "Station", 140, 12,100);
            CreateTile("Trịnh Minh Thế", true, "Pink", 140, 13,160);
            CreateTile("Lý Thái Tổ", true, "Pink", 160, 14,160);
            CreateTile("Bến xe Lam Chợ Lớn", true, "Station", 200, 15,100);
            CreateTile("Đại lộ Hùng Vương", true, "Orange", 180, 16,200);
            CreateTile("Khí vận", false, "Opportunity", 0, 17,0);
            CreateTile("Gia Long", true, "Orange", 180, 18,200);
            CreateTile("Bến Bạch Đằng", true, "Orange", 200, 19,200);
            CreateTile("Sân bay", false, "Null", 0, 20,0);
            CreateTile("Đường Công Lý", true, "Red", 220, 21,240);
            CreateTile("Cơ hội", false, "Opportunity", 0, 22,0);
            CreateTile("Đại lộ thống nhất", true, "Red", 220, 23,240);
            CreateTile("Đại lộ Cộng Hòa", true, "Red", 240, 24,240);
            CreateTile("Bến xe An Đông", true, "Station", 200, 25,100);
            CreateTile("Đại lộ Hồng Thập Tự", true, "Yellow", 260, 26,280);
            CreateTile("Đại lộ Hai Bà Trưng", true, "Yellow", 260, 27,280);
            CreateTile("Công ty thủy cục", true, "Station", 150, 28,100);
            CreateTile("Xa lộ Biên Hòa", true, "Yellow", 280, 29,280);
            CreateTile("VÔ TÙ", false, "Null", 0, 30,0);
            CreateTile("Phan Thanh Giảm", true, "Green", 300, 31,320);
            CreateTile("Lê Văn Duyệt", true, "Green", 300, 32,320);
            CreateTile("Khí vận", false, "Opportunity", 0, 33,0);
            CreateTile("Nguyễn Thái Học", true, "Green", 320, 34,320);
            CreateTile("Tân Kì Tân Quý", true, "Station", 400, 35,100);
            CreateTile("Cơ hội", false, "Opportunity", 0, 36,0);
            CreateTile("Nha Trang", true, "Blue", 350, 37,400);
            CreateTile("Thuế lương bổng", false, "White", 0, 38,400);
            CreateTile("Cố Đô Huế", true, "Blue", 400, 39,400);

            Players[0] = new Player();
            Players[1] = new Player();
            #endregion //Cập nhật giao diện người chơi 
        }

        private void Game_Load(object sender, EventArgs e)
        {
            //Nếu chế độ nhiều người chơi 
            if (Gamemodes.Multiplayer){
                try
                {
                    //Hiển thị form kết nối
                    Connection connection = new Connection();
                    connection.ShowDialog();
                    //Nếu chọn Cancel thì hủy kết nối rồi quay về MainMenu chính 
                    if (connection.DialogResult is DialogResult.Cancel){
                        this.Close();
                        return;
                    }
                    //Hiển thị thông điệp là đang đợi người chơi thứ 2 nên vô hiệu hóa các nút chơi
                    currentPlayersTurn_textbox.Text = "Waiting for second player ...........";
                    throwDiceBtn.Enabled = false;
                    buyBtn.Enabled = false;
                    endTurnBtn.Enabled = false;
                    if (Gamemodes.Create){
                        Startbtn.Visible = true;
                        Startbtn.Enabled = false;
                    }
                    else Startbtn.Visible = false;
                    //Kết nối tới Server
                    try
                    {
                        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        clientSocket.Connect(ConnectionOptions.Instance.IP, 11000);
                        Stream = new NetworkStream(clientSocket);
                    }
                    catch
                    {
                        MessageBox.Show("Can't connect to server."
                                        + Environment.NewLine
                                        + "Server is not working");
                        this.Disconnect();
                        Environment.Exit(0);
                    }
                    //Tạo luồng nhận dữ liệu từ Server 
                    Thread receiveThread = new Thread(ReceiveMessage);
                    receiveThread.Start();
                    //Gửi thông điệp "Tạo phòng" hoặc "Tham gia"
                    if (Gamemodes.Create) SendMessageToServer("Create" + ";" + RoomInfo.Instance.Room);
                    else SendMessageToServer("Join" + ";" + RoomInfo.Instance.Room);
                    ConnectionOptions.Instance.Connect = true;

                    // Tạm dừng trong 500ms để nhận phản hồi từ server
                    Thread.Sleep(500);
                    this.Hide();
                    //Hiển thị Form chọn màu 
                    ColorChoosing colorChoosing = new ColorChoosing();
                    colorChoosing.ShowDialog();
                    //Nếu chọn Thoát thì ngắt kết nối và đóng ứng dụng
                    if (colorChoosing.DialogResult is DialogResult.Cancel){
                        if (!Gamemodes.Create)
                            messagetype = "Disconected";
                        else messagetype = "Exit";
                        SendMessageToServer(messagetype + ";" + RoomInfo.Instance.Room);
                        this.Disconnect();
                        Environment.Exit(0);
                    }
                    else this.Show(); //Hiển thị bàn cờ

                    //Gửi tên người chơi đến server
                    SendMessageToServer("Connect" + ";" + RoomInfo.Instance.Room + ";"+ RoomInfo.Instance.PlayerName + ";" + RoomInfo.Instance.UserName + ";");
                    //Xác định người chơi hiện tại và đánh dấu họ đã kết nối 
                    string[] player = RoomInfo.Instance.PlayerName.Split(';');
                    switch (player[0])
                    {
                        case "Red":
                            Player1Name.Text = RoomInfo.Instance.UserName;
                            CurrentPlayerId = 0;
                            colorLb.Text = RoomInfo.Instance.Room;
                            colorLb.BackColor = Color.Red;
                            break;
                        case "Blue":
                            Player2Name.Text = RoomInfo.Instance.UserName;
                            CurrentPlayerId = 1;
                            colorLb.Text = RoomInfo.Instance.Room;
                            colorLb.BackColor = Color.Blue;
                            break;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                UpdatePlayersStatusBoxes();
                buyBtn.Enabled = false;
            }
            else{
                Startbtn.Visible = false;
                Player1Name.Text = "RED";
                Player2Name.Text = "BLUE";
            }

        }

        //Hàm khởi tạo giá trị cho 1 ô trên bàn cờ
        private void CreateTile(string tileName, bool tileBuyable, string tileColor, int tilePrice, int tilePosition,int rent)
        {
            Property property = new Property
            {
                Name = tileName,
                Color = tileColor,
                Buyable = tileBuyable,
                Price = tilePrice,
                Rent = rent
            };
            //Gắn ô trên bàn cờ vào vị trí tương ứng trong mảng Properties 
            Properties[tilePosition] = property;
        }
        //Chuyển danh sách tài sản thành 1 chuỗi để chuyển đến server
        private string PropertiesToString(int[] propertyList)
        {
            var tempString = "";
            //Chạy qua ds tài sản, sau đó thêm tên, màu vào chuỗi 
            for (var i = 0; i < 40; i++)
                if (propertyList[i] != 0)
                    tempString = tempString + Properties[propertyList[i]].Name + ": " +Properties[propertyList[i]].Rent + "\n";
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
                "Remaining money: " + Players[0].Balance + "\n"
                + "Property :\n" + PropertiesToString(Players[0].PropertiesOwned);
                });
                }
            else
            {
                redPlayerStatusBox_richtextbox.Text =
               "Remaining money: " + Players[0].Balance + "\n"
              + "Property :\n" + PropertiesToString(Players[0].PropertiesOwned);
            }
            if (bluePlayerStatusBox_richtextbox.InvokeRequired)
            {
                bluePlayerStatusBox_richtextbox.Invoke((MethodInvoker)delegate
                {
                    bluePlayerStatusBox_richtextbox.Text =
                         "Remaining money: " + Players[1].Balance + "\n"
                        + "Property :\n" + PropertiesToString(Players[1].PropertiesOwned);
                });
            }
            else
            {
                bluePlayerStatusBox_richtextbox.Text =
                         "Remaining money: " + Players[1].Balance + "\n"
                        + "Property :\n" + PropertiesToString(Players[1].PropertiesOwned);
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
                        Player1Name.Text + " You're in jail! Your can't do any action this turn "; break;
                case 1:
                    currentPlayersTurn_textbox.Text =
                        Player2Name.Text + " You're in jail! Your can't do any action this turn ";
                    break;
            }
            //Nếu người chơi đã vào tù 3 lần thì thả người chơi ra 
            if (Players[CurrentPlayerId].Jail != 4) 
                return;
            //Sau khi thả người chơi thì hiển thị thông báo trên dao diện
            Players[CurrentPlayerId].InJail = false;
            Players[CurrentPlayerId].Jail = 0;
            throwDiceBtn.Enabled = true;
            switch (CurrentPlayerId)
            {
                case 0:
                    currentPlayersTurn_textbox.Text =
                        Player1Name.Text + " you’re free! ";
                    break;
                case 1:
                    currentPlayersTurn_textbox.Text =
                        Player2Name.Text + " you’re free! ";
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
        //Vẽ hình tròn cho tài sản của người chơi sở hữu trên bàn cờ
        private void DrawCircle(int position, int playerId)
        {
            //Lấy tọa độ x, y của các ô trên bàn cờ 
            int x = Tile[position].Location.X, y = Tile[position].Location.Y;
            switch (playerId)
            {
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
                            Size = new Size(30,30),
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
            timeLabel.Text = $"{timeLeft} seconds"; // Hiển thị thời gian còn lại
        }
        //Nhận các tin nhắn từ server và xử lý
        private void ReceiveMessage()
        {
            //Nhận tin nhắn nếu Receiving là true
            while (Receiving)
                try
                {
                    byte[] data = new byte[256];
                    StringBuilder builder = new StringBuilder();
                    do{
                        var bytes = Stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (Stream.DataAvailable);
                    String message = builder.ToString();
                    //Phân tích tin nhắn nhận được bằng dấu ";"
                    string[] parts = message.Split(';');
                    switch (parts[0]) // Xem loại tin nhắn
                    {
                        case "Update":
                           
                                Player1Name.Invoke((MethodInvoker)delegate
                                {
                                    Player1Name.Text = parts[2];
                                });
                                Player2Name.Invoke((MethodInvoker)delegate
                                {
                                    Player2Name.Text = parts[3];
                                });
                                if (Gamemodes.Create){ //Nếu là chủ phòng thì được phép ấn START
                                    currentPlayersTurn_textbox.Invoke((MethodInvoker)delegate
                                    {
                                        Startbtn.Enabled = true;
                                        currentPlayersTurn_textbox.Text = "Click button START to start the game";
                                    });
                                }
                                else currentPlayersTurn_textbox.Invoke((MethodInvoker)delegate{
                                    currentPlayersTurn_textbox.Text = "Waiting for host to start the game.........";
                                });
                                Players[0].Name = parts[2];
                                Players[1].Name = parts[3];
                                UpdatePlayersStatusBoxes();

                            break;
                        case "Start":
                                ConnectionOptions.Instance.Started = true; //Đánh dấu phòng đã bắt đầu
                                if (CurrentPlayerId == Convert.ToInt32(parts[3])){
                                    currentPlayersTurn_textbox.Invoke((MethodInvoker)delegate
                                    {
                                        isTurn = true;
                                        timeLeft = turnTimeLimit;
                                        if (turnTimer == null){
                                            turnTimer = new System.Windows.Forms.Timer();
                                            turnTimer.Interval = 1000; // 1 giây (1000ms)
                                            turnTimer.Tick += new EventHandler(timer1_Tick);
                                        }
                                        UpdateTimeDisplay();
                                        turnTimer.Start();
                                        currentPlayersTurn_textbox.Text = "Throw dice";
                                        throwDiceBtn.Enabled = true;
                                        buyBtn.Enabled = false;
                                        endTurnBtn.Enabled = false;
                                    });
                                }
                                else{
                                    currentPlayersTurn_textbox.Invoke((MethodInvoker)delegate
                                    {
                                        isTurn = false ;
                                        timeLeft = turnTimeLimit;
                                        if (turnTimer == null){
                                            turnTimer = new System.Windows.Forms.Timer();
                                            turnTimer.Interval = 1000; // 1 giây (1000ms)
                                            turnTimer.Tick += new EventHandler(timer1_Tick);
                                        }
                                        UpdateTimeDisplay();
                                        turnTimer.Start();
                                        currentPlayersTurn_textbox.Text = Player1Name.Text + " is making turn, just waiting....";
                                        Startbtn.Enabled = false;
                                    });
                                }
                            break;
                        case "Send":
                                string decodedMessage = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(parts[3]));
                                messageRTB.Invoke((MethodInvoker)delegate
                                {
                                  messageRTB.AppendText(parts[2] + ": "+decodedMessage+ Environment.NewLine);
                                });
                            break;
                        case "Exit":
                                messagetype = "Exit";
                                this.Invoke((MethodInvoker)delegate
                                {
                                    MessageBox.Show("Your opponent has left.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    SendMessageToServer(messagetype + ";" + RoomInfo.Instance.Room);
                                    this.Disconnect();
                                    Environment.Exit(0);
                                });
                            break;
                        case "Exit lobby": //Thoát phòng mà không xóa phòng ( phòng chưa bắt đầu , không phải chủ phòng )
                                Startbtn.Enabled = false;
                                switch(CurrentPlayerId)
                                {
                                    case 0:
                                    RoomInfo.Instance.NameBlueIsTaken = false;
                                        Player2Name.Invoke((MethodInvoker)delegate
                                        {
                                            currentPlayersTurn_textbox.Text = "Waiting for second player";
                                            Player2Name.Text = "Waiting for player ...";
                                        });
                                        break;
                                    case 1:
                                    RoomInfo.Instance.NameRedIsTaken = false;
                                        Player1Name.Invoke((MethodInvoker)delegate
                                        {
                                            currentPlayersTurn_textbox.Text = "Waiting for second player";
                                            Player1Name.Text = "Waiting for player ...";
                                        });
                                        break;
                                }    
                            break;
                            //Cập nhật thông tin người chơi.
                        case "Result":
                                isTurn = true; //Khi nhận được tin nhắn, đánh dấu là đã tới lượt
                                int temp = Convert.ToInt32(parts[3]); //kiểm tra xem nếu người chơi hiện tại có số id = thông tin nhận được
                                if (Convert.ToInt32(parts[3]) == CurrentPlayerId){
                                    currentPlayersTurn_textbox.Invoke((MethodInvoker)delegate
                                    {
                                        // Cập nhật trạng thái của textbox hiển thị lượt của người chơi hiện tại
                                        //bắt đầu điếm thời gian
                                        timeLeft = turnTimeLimit;
                                        if (turnTimer == null){
                                            turnTimer = new System.Windows.Forms.Timer();
                                            turnTimer.Interval = 1000; // 1 giây (1000ms)
                                            turnTimer.Tick += new EventHandler(timer1_Tick);
                                        }
                                        UpdateTimeDisplay();
                                        turnTimer.Start();
                                        currentPlayersTurn_textbox.Text = "Your turn";
                                        if (Players[CurrentPlayerId].InJail){
                                            InJail();
                                        }
                                        else{
                                            //cập nhật trang thái các nút
                                            throwDiceBtn.Enabled = true;
                                            buyBtn.Enabled = false;
                                            endTurnBtn.Enabled = false;
                                        }
                                    });
                                }

                                // Tạo một đối tượng Playerinfomation để lưu trữ thông điệp tài sản nhận được
                                Playerinfomation Playerinfomation = new Playerinfomation();

                                // Lấy vị trí kết thúc lượt đi từ tin nhắn
                                Playerinfomation.EndPosition = Convert.ToInt32(parts[4]);
                                // Lấy số tiền sau lượt đi từ tin nhắn
                                String stringBalance = parts[5];
                                Playerinfomation.Balance = Convert.ToInt32(stringBalance);
                                // Lấy chuỗi tài sản (đất) hiện có từ tin nhắn
                                String stringPropertiesOwned = parts[6];
                                if (stringPropertiesOwned != null){
                                    //tách chuỗi tài sản bằng dấu ' '
                                    int[] tempArrayOfPropertiesOwned = stringPropertiesOwned
                                        .Split(' ')
                                        .Where(x => !string.IsNullOrWhiteSpace(x))
                                        .Select(x => int.Parse(x))
                                        .ToArray();
                                    for (int k = 0; k < tempArrayOfPropertiesOwned.Length; k++)
                                        Playerinfomation.PropertiesOwned[k] = tempArrayOfPropertiesOwned[k]; //cập nhật tài sản đang sở hữu cho người chơi
                                }
                                UpdatePlayerStatus(temp, Playerinfomation);
                                if (Convert.ToInt32(stringBalance) < 0){
                                    Win();
                                }
                            break;
                            //sự kiện trả tiền khi đi lên đất người khác
                        case "Rent":
                                string sumOfRentString = parts[3];
                                int sumOfRent = Convert.ToInt32(sumOfRentString);
                                switch (parts[1])
                                {
                                    case "Red":
                                        ChangeBalance(Players[0], -sumOfRent);
                                        ChangeBalance(Players[1], sumOfRent);
                                        this.Invoke((MethodInvoker)delegate
                                        {
                                           currentPlayersTurn_textbox.Text =  Player1Name.Text + "pay rent for " + Player2Name.Text + sumOfRent;
                                        });
                                        break;
                                    case "Blue":
                                        ChangeBalance(Players[1], -sumOfRent);
                                        ChangeBalance(Players[0], sumOfRent);
                                        this.Invoke((MethodInvoker)delegate
                                        {
                                            currentPlayersTurn_textbox.Text = Player2Name.Text + "pay rent for " + Player1Name.Text + sumOfRent;
                                        });
                                        break;
                                }
                            //}
                            break;
                            //thông báo chọn quân cờ
                        case "Red pawn already selected":
                            RoomInfo.Instance.NameRedIsTaken = true;
                            RoomInfo.Instance.choosenPlayer = parts[2];
                            break;
                        case "Blue pawn already selected":
                            RoomInfo.Instance.NameBlueIsTaken = true;
                            RoomInfo.Instance.choosenPlayer = parts[2];
                            break;
                          //thông báo phòng đầy
                        case "Room is full":
                            this.Invoke((MethodInvoker)delegate
                            {
                                Receiving = false;
                                Disconnect();
                                MessageBox.Show("Room is full.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                Environment.Exit(0);
                            });
                            break;
                            //Thông báo không tồn tại phòng
                        case "Room not found":
                            Receiving = false;
                            this.Invoke((MethodInvoker)delegate
                            {
                                Disconnect();
                                MessageBox.Show("Game room not found.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                Environment.Exit(0);
                            });
                            break;
                            //Thông báo phòng đã tồn tại.
                        case "Room already exists":
                            Receiving = false;
                            this.Invoke((MethodInvoker)delegate
                            {
                                Disconnect();
                                MessageBox.Show("The room already exists. Please create a new one.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                Environment.Exit(0);
                            });
                            break;
                            //cập nhật vị trí
                        case "Location":
                            MoveOpponentIcon(parts[2], Convert.ToInt32(parts[3]));
                            break;
                    }
                }
                catch (Exception e) 
                {
                    MessageBox.Show(e.Message,"Aler",MessageBoxButtons.OK);
                    SendMessageToServer("Exit" + ";" + RoomInfo.Instance.Room);
                    this.Disconnect();
                    Environment.Exit(0);
                }
        }
        //Hàm được gọi khi người chơi thua cuộc
        private void Lose(){
            messagetype = "Lose";
            this.Invoke((MethodInvoker)delegate
            {
                counter++; 
                if (counter >= 3)
                {
                    if (MessageBox.Show("You have passed turn 3 times, you lose", "", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        SendMessageToServer(messagetype + ";" + RoomInfo.Instance.UserName + ";" + RoomInfo.Instance.Room);
                        this.Disconnect();
                        Environment.Exit(0);
                    }
                }
                else
                {
                    if (MessageBox.Show("You lose! Better next time!", "", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        SendMessageToServer(messagetype + ";" + RoomInfo.Instance.UserName + ";" + RoomInfo.Instance.Room);
                        this.Disconnect();
                        Environment.Exit(0);
                    }
                }
            });
        }
        //Hàm gọi khi người chơi thắng
        private void Win(){
            messagetype = "Win";
            this.Invoke((MethodInvoker)delegate
            {
                if (MessageBox.Show("You Win! Congratulations!", "Notification", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    SendMessageToServer(messagetype + ";" + RoomInfo.Instance.UserName + ";" + RoomInfo.Instance.Room);
                    this.Disconnect();
                    Environment.Exit(0);
                }
            });

        }
        //Hàm cập nhật thông tin người chơi
        private void UpdatePlayerStatus(int playerId, Playerinfomation Playerinfomation){
            int temp = CurrentPlayerId;
            CurrentPlayerId = CurrentPlayerId is 0 ? 1 : 0;
            // Di chuyển biểu tượng của người chơi đến vị trí kết thúc lượt đi
            Invoke((MethodInvoker)delegate
            {
                MoveIcon(Playerinfomation.EndPosition);
            });
            // Cập nhật vị trí và số dư của người chơi
            Players[CurrentPlayerId].Position = Playerinfomation.EndPosition;
            Players[CurrentPlayerId].Balance = Playerinfomation.Balance;

            // Cập nhật danh sách tài sản được sở hữu của người chơi
            int i = 0;
            foreach (var item in Playerinfomation.PropertiesOwned)
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
        //Phương thức ngắt kết nối 
        private void Disconnect()
        {
            Receiving = false;
            try
            {
                Stream?.Close();
                Stream = null;
                if (clientSocket != null)
                {
                    clientSocket.Shutdown(SocketShutdown.Both); // Đóng cả gửi và nhận
                    clientSocket.Close();
                    clientSocket = null; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while disconnecting: {ex.Message}");
            }
        }
        //di chuyển quân cờ
        private void MoveOpponentIcon(string color, int position)
        {
            int x, y;
            switch(color)
            {
                case "Red":
                    x = Tile[position].Location.X;
                    y = Tile[position].Location.Y;
                    redPawnIcon.Invoke((MethodInvoker)delegate
                    {
                        redPawnIcon.Location = new Point(x, y);
                    });
                    break;
                case "Blue":
                    x = Tile[position].Location.X;
                    y = Tile[position].Location.Y;
                    bluePawnIcon.Invoke((MethodInvoker)delegate
                    {
                        bluePawnIcon.Location = new Point(x, y);
                    });
                    break;
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
            messagetype = "Send";
            string message = messageTb.Text.Trim(); // loại bỏ khoảng trắng
            if (string.IsNullOrEmpty(message))
                return;
            string ecoding = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message));
            if (Gamemodes.Multiplayer) SendMessageToServer(messagetype + ";" + RoomInfo.Instance.Room + ";" + RoomInfo.Instance.UserName + ";" + ecoding);
            else messageRTB.Text += message +"\r\n";
            messageTb.Text = "";
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(Gamemodes.Singleplayer)
            {
                Environment.Exit(0);
            }    
            if (ConnectionOptions.Instance.Started)
            {
                QuitGame();
 
            }
            else if (Gamemodes.Create && ConnectionOptions.Instance.Connect)
            {
                QuitGame();
            }
            else if(!ConnectionOptions.Instance.Started && ConnectionOptions.Instance.Connect)
            {
                messagetype = "Exit lobby";
                SendMessageToServer(messagetype + ";"+ RoomInfo.Instance.Room + ";" + RoomInfo.Instance.PlayerName);
                this.Disconnect();
                Environment.Exit(0);
            }
        }

        //Animation di chuyển vị trí
        private async Task<int> MoveTileByTile(int from, int to){
            messagetype = "Location";
            // Nếu vị trí đích nhỏ hơn 40 (nằm trong phạm vi của bảng), di chuyển từ ô hiện tại đến ô đích
            if (to < 40){
                for (var i = from; i <= to; i++){
                    await Task.Delay(200);
                    if(Gamemodes.Multiplayer) SendMessageToServer(messagetype + ";" + RoomInfo.Instance.Room + ";"+ RoomInfo.Instance.PlayerName + ";" + i);
                    MoveIcon(i);
                    endTurnBtn.Enabled = false;
                }
                endTurnBtn.Enabled = true;
            }
            else{
                // Nếu vị trí đích lớn hơn hoặc bằng 40, di chuyển từ ô hiện tại đến ô cuối cùng (39),
                // sau đó di chuyển từ ô đầu tiên (0) đến ô đích với phần dư của vị trí đích sau khi chia cho 40
                for (var i = from; i <= 39; i++){
                    await Task.Delay(200);
                    if (Gamemodes.Multiplayer) SendMessageToServer(messagetype + ";" + RoomInfo.Instance.Room + ";" + RoomInfo.Instance.PlayerName + ";" + CurrentPlayerId + ";" + i);
                    MoveIcon(i);
                    endTurnBtn.Enabled = false;
                }
                for (var i = 0; i <= to - 40; i++){
                    await Task.Delay(200);
                    if (Gamemodes.Multiplayer) SendMessageToServer(messagetype + ";" + RoomInfo.Instance.Room + ";" + RoomInfo.Instance.PlayerName + ";" + CurrentPlayerId + ";" + i);
                    MoveIcon(i);
                    endTurnBtn.Enabled = false;
                }
                endTurnBtn.Enabled = true;
            }
            return 1;
        }

        public void Startbtn_Click(object sender, EventArgs e){
            messagetype = "Start";
            SendMessageToServer(messagetype + ";" + RoomInfo.Instance.Room + ";" + RoomInfo.Instance.PlayerName + ";" + 0);
            Startbtn.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e){
            timeLeft--;

            // Cập nhật giao diện
            UpdateTimeDisplay();

            // Nếu hết thời gian, chuyển lượt
            if (timeLeft <= 0)
            {
                if (isTurn)
                {
                    counter++;
                    EndTurn(); // Kết thúc lượt của người chơi hiện tại
                    isTurn = false;
                }
            }
        }
        private void EndTurn(){
            messagetype = "Result";
            if (Gamemodes.Multiplayer)
            {
                if (counter >= 3) Players[CurrentPlayerId].Balance = -1;
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
                });
                    currentPositionInfo_richtextbox.Text = string.Empty;
                string turnLogString = string.Empty;
                switch (CurrentPlayerId)
                {
                    case 0:
                        turnLogString = messagetype + ";" + RoomInfo.Instance.Room + ";" + RoomInfo.Instance.PlayerName + ";" + "1" + ";"; //" Kết quả lượt đi của Đỏ và lượt người chơi tiếp theo"
                        break;
                    case 1:
                        turnLogString = messagetype + ";" + RoomInfo.Instance.Room + ";" + RoomInfo.Instance.PlayerName + ";" + "0" + ";";// kết quả lượt đi của xanh 
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
                    currentPlayersTurn_textbox.Text = Player2Name.Text + " is making turn, waiting.........";
                    SendMessageToServer(turnLogString);
                }
                else
                {
                    currentPlayersTurn_textbox.Text = Player1Name.Text + " is making turn, waiting..........";
                    SendMessageToServer(turnLogString);
                }


                if (Players[CurrentPlayerId].Balance < 0)
                    Lose();
                if (counter >= 3) Lose();
                else
                {
                    throwDiceBtn.Enabled = false;
                    buyBtn.Enabled = false;
                    endTurnBtn.Enabled = false;
                }
            }
        }
        //Xử lý khi nút ném xúc xắc 
        private void ThrowDiceBtn_Click(object sender, EventArgs e){
            //Hiển thị thông tin của người chơi hiện tại 
            switch (CurrentPlayerId)
            {
                case 0:
                    currentPlayersTurn_textbox.Text =  Player1Name.Text + "'s Turn ";
                    break;
                case 1:
                    currentPlayersTurn_textbox.Text = Player2Name.Text + "'s Turn";
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
            //endTurnBtn.Enabled = true;

            //Cập nhật trạng thái của người chơi 
            UpdatePlayersStatusBoxes();

            //Ném xúc sắc 
            Random rand = new Random();
            int firstDice =rand.Next(1, 6);
            int secondDice =rand.Next(1, 6);
            Dice = firstDice + secondDice;
            //Hiển thị kết quả xức sắc 
            whatIsOnDices_textbox.Text = "Result: " + firstDice + " and " + secondDice + ". Total: " + Dice + ". ";
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

            currentPositionInfo_richtextbox.Text = "Position " + CurrentPosition;
            currentPositionInfo_richtextbox.AppendText("\r\n" + Properties[CurrentPosition].Name);
            currentPositionInfo_richtextbox.AppendText(" Price " + Properties[CurrentPosition].Price);

            if (visitedJailExploration) 
                currentPositionInfo_richtextbox.AppendText("\r\n" + "You visit prison ");

            if (visitedTaxTile) 
                currentPositionInfo_richtextbox.AppendText("\r\n" + "You have paid taxes");

            if (visitedGo) 
                currentPositionInfo_richtextbox.AppendText("\r\n" + "You get 200$ when pass \"GO\". ");

            if (visitedFreeParking) 
                currentPositionInfo_richtextbox.AppendText("\r\n" + "Just chill...");

            if (landedOpportunity)
                if (CurrentPosition == 2 || CurrentPosition == 17 || CurrentPosition == 33)
                    currentPositionInfo_richtextbox.AppendText("\r\n" + "You get " + Convert.ToString(OppResult) + " in \"Khí Vận\".");
                else
                    currentPositionInfo_richtextbox.AppendText("\r\n" + "You get " + Convert.ToString(OppResult) + " in \"Cơ Hội\".");

            if (goingToJail)
            {
                currentPositionInfo_richtextbox.AppendText("\r\n" + "You are in Jail.");
                switch (CurrentPlayerId)
                {
                    case 0:
                        currentPlayersTurn_textbox.Text = Player1Name.Text + ", You are in Jail , your turn will passed ";
                        break;
                    case 1:
                        currentPlayersTurn_textbox.Text = Player2Name.Text + " You are in Jail, your turn will passed ";
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
                        string rentMessage = "Rent"+";" + RoomInfo.Instance.Room + ";" + RoomInfo.Instance.PlayerName + ";"+ GetRent(Dice);
                        SendMessageToServer(rentMessage);
                    }
                    break;
                case 1:
                    ChangeBalance(Players[1], -GetRent(Dice));
                    ChangeBalance(Players[0], GetRent(Dice));
                    if (Gamemodes.Multiplayer)
                    {
                        string rentMessage = "Rent" + ";" + RoomInfo.Instance.Room + ";" + RoomInfo.Instance.PlayerName + ";"+ GetRent(Dice);
                        SendMessageToServer(rentMessage);
                    }
                    break;
            }
            switch (CurrentPlayerId)
            {
                case 0:
                    currentPlayersTurn_textbox.Text = Player1Name.Text + " You just entered land owned by another player and had to pay ";
                    break;
                case 1:
                    currentPlayersTurn_textbox.Text = Player2Name.Text + " You just entered land owned by another player and had to pay ";
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
                    Players[CurrentPlayerId].PropertiesOwned[CurrentPosition] = CurrentPosition;
                    Properties[CurrentPosition].Owned = true;
                    Players[CurrentPlayerId].NumberOfPropertiesOwned++;
                    UpdatePlayersStatusBoxes();
                    buyBtn.Enabled = false;
                    DrawCircle(CurrentPosition, CurrentPlayerId);
                }
                else 
                    currentPlayersTurn_textbox.Text = "You're not enough money";
            else 
                currentPlayersTurn_textbox.Text = "You can't do that";
        }

        private void messageTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Ngăn Enter tạo dòng mới trong TextBox
                e.SuppressKeyPress = true;

                // Gọi hàm sendBt_Click để gửi tin nhắn
                sendBt.PerformClick();
            }
        }

        private void QuitGame()
        {
            if (MessageBox.Show("Are you want to exit", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Gamemodes.Multiplayer)
                    SendMessageToServer("Exit" + ";" + RoomInfo.Instance.Room);
                this.Disconnect();
                Environment.Exit(0);
            }
        }
        private void QuitGameBtn_Click(object sender, EventArgs e)
        {
            if(Gamemodes.Singleplayer)
            {
                Environment.Exit(0);
            }    
            else if (ConnectionOptions.Instance.Started)
            {
                QuitGame();
            }
            else if(Gamemodes.Create)
            {
                QuitGame();
            }
            else
            {
                MessageBox.Show("Are you want to exit","Exit",MessageBoxButtons.YesNo);
                SendMessageToServer("Exit lobby" + ";" + RoomInfo.Instance.Room + ";" + RoomInfo.Instance.PlayerName);
                this.Disconnect();
                Environment.Exit(0);
            } 
                
        }

        private void EndTurnBtn_Click(object sender, EventArgs e)
        {
            messagetype = "Result";
            if (Gamemodes.Multiplayer)
            {
                isTurn = false;
                EndTurn();
            }
            if (Gamemodes.Singleplayer)
            {
                if(Players[CurrentPlayerId].Balance<0 )
                {
                    switch (CurrentPlayerId)
                    {
                        case 0:
                            if (MessageBox.Show("The blue player has won!", "Message", MessageBoxButtons.OK) is DialogResult.OK) Application.Exit();
                            break;
                        case 1:
                            if (MessageBox.Show("The red player has won!", "Message", MessageBoxButtons.OK) is DialogResult.OK) Application.Exit();
                            break;
                    }
                }    
                CurrentPlayerId = CurrentPlayerId is 0 ? 1 : 0;
                switch (CurrentPlayerId)
                {
                    case 0:
                        currentPlayersTurn_textbox.Text = "Red turn. ";
                        break;
                    case 1:
                        currentPlayersTurn_textbox.Text = "Blue turn. ";
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
        public void SendMessageToServer(string message)
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
                MessageBox.Show("Error when send message to server: " + ex.Message);
            }
        }
    }
}