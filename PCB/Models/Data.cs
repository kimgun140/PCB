using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PCB.VIEW;

namespace PCB.Models
{
    public class PCBinfo
    {
        // 1차 검수 정상 / 불량 상태 확인
        public int Status { get; set; }

        public int MCU { get; set; }
        public int LTC { get; set; }
        public int ADC { get; set; }
        public int DAC { get; set; }
        public int XTR { get; set; }
        public int LED1 { get; set; }
        public int LED2 { get; set; }

    }
    public class PCBinfo2
    {
        public string num { get; set; }
        public string Name { get; set; }
        public bool CheckBoxStatus { get; set; }

    }
}
