﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Player
    {
        public string Name;
        public int TurnPassed = 0;
        //Cho biết số dư, số lượng tài sản, số lần vào tù, vị trí hiện tại 
        public int Balance = 500, NumberOfPropertiesOwned, Jail, Position;
        //Tình trạng giam giữ/ thua cuộc
        public bool InJail;
        //ID của tài sản 
        public readonly int[] PropertiesOwned = new int[40];
    }
}
