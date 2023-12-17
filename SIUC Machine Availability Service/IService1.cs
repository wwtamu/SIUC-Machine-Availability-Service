using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SIUCMachineAvailabilityService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool Login(string machine_name);

        [OperationContract]
        bool Logout(string machine_name);

        [OperationContract]
        bool Reset(string machine_name);

        [OperationContract]
        bool Initialize(string machine_name, string mac_address, string ip_address);

        [OperationContract]
        List<bool> GetComputersAvailability(string lab_name);

        [OperationContract]
        List<string> GetComputersStatus(string lab_name);

        [OperationContract]
        bool GetLabAvailability(string lab_name);
        
        [OperationContract]
        List<LabObject> GetAllLabs();

        [OperationContract]
        List<MachineObject> GetComputersInLab(string lab_name);
    }

    [DataContract]
    public class EventObject
    {                                           // Microsoft SQL dbo.Event
        private int event_id;                   // eid   int primary key NOT NULL
        private DateTime date_time;             // edate datetime NOT NULL
        private int machine_id;                 // fmid  int references dbo.Machines(mid) NOT NULL
        private string e_string;                // event varchar(10) {"login", "logoff", "reset"}        

        [DataMember]
        public int EventId
        {
            get { return event_id; }
            set { event_id = value; }
        }
        [DataMember]
        public DateTime EventDateTime
        {
            get { return date_time; }
            set { date_time = value; }
        }
        [DataMember]
        public int MachineId
        {
            get { return machine_id; }
            set { machine_id = value; }
        }
        [DataMember]
        public string EventString
        {
            get { return e_string; }
            set { e_string = value; }
        }
    }

    [DataContract]
    public class AvailabilityObject
    {                                           // Microsoft SQL dbo.Machines
        private int availability_id;            // aid   int primary key NOT NULL
        private DateTime date_time;             // adate datetime NOT NULL
        private int machine_id;                 // fmid  int references dbo.Machines(mid) NOT NULL
        private string machine_last_known_ip;   // mtype varchar(15)
        private bool available;                 // mavailable bit NOT NULL

        [DataMember]
        public int AvailabilityId
        {
            get { return availability_id; }
            set { availability_id = value; }
        }
        [DataMember]
        public DateTime AvailabilityDateTime
        {
            get { return date_time; }
            set { date_time = value; }
        }
        [DataMember]
        public int MachineId
        {
            get { return machine_id; }
            set { machine_id = value; }
        }
        [DataMember]
        public string MachineLastKnownIPAddress
        {
            get { return machine_last_known_ip; }
            set { machine_last_known_ip = value; }
        }
        [DataMember]
        public bool Availability
        {
            get { return available; }
            set { available = value; }
        }
    }

    [DataContract]
    public class MachineObject
    {                                   // Microsoft SQL dbo.Machines
        private int machine_id;         // mid   int primary key NOT NULL
        private int lab_id;             // flid  int references dbo.Labs(lid) NOT NULL        
        private string machine_name;    // mname varchar(100) NOT NULL
        private string machine_mac;     // mmac varchar(20) NOT NULL
        private string machine_lkip;    // mlkip varchar(20) NOT NULL
        private string machine_label;   // mlabel varchar(20)
        private string machine_type;    // mtype varchar(50)

        [DataMember]
        public int MachineId
        {
            get { return machine_id; }
            set { machine_id = value; }
        }
        [DataMember]
        public int LabId
        {
            get { return lab_id; }
            set { lab_id = value; }
        }
        [DataMember]
        public string MachineName
        {
            get { return machine_name; }
            set { machine_name = value; }
        }
        [DataMember]
        public string MachineMac
        {
            get { return machine_mac; }
            set { machine_mac = value; }
        }
        [DataMember]
        public string MachineIP
        {
            get { return machine_lkip; }
            set { machine_lkip = value; }
        }
        [DataMember]
        public string MachineLabel
        {
            get { return machine_label; }
            set { machine_label = value; }
        }
        [DataMember]
        public string MachineType
        {
            get { return machine_type; }
            set { machine_type = value; }
        }
    }

    [DataContract]
    public class LabObject
    {                                   // Microsoft SQL dbo.Labs
        private int lab_id;             // lid    int primary key NOT NULL
        private string lab_name;        // lname  varchar(100)
        private string lab_building;    // lbuilding varchar(100) NOT NULL
        private int lab_floor;          // lfloor int 
        private string lab_room;        // lroom  varchar(20)
        private string lab_open;        // lopen  time NOT NULL
        private string lab_close;       // lclose time NOT NULL
        private string lab_status;      // lstat  varchar(10) NOT NULL
        private int machine_count;      // mcount int NOT NULL

        [DataMember]
        public int LabId
        {
            get { return lab_id; }
            set { lab_id = value; }
        }
        [DataMember]
        public string LabName
        {
            get { return lab_name; }
            set { lab_name = value; }
        }
        [DataMember]
        public string LabBuilding
        {
            get { return lab_building; }
            set { lab_building = value; }
        }
        [DataMember]
        public int LabFloor
        {
            get { return lab_floor; }
            set { lab_floor = value; }
        }
        [DataMember]
        public string LabRoom
        {
            get { return lab_room; }
            set { lab_room = value; }
        }
        [DataMember]
        public string LabOpen
        {
            get { return lab_open; }
            set { lab_open = value; }
        }
        [DataMember]
        public string LabClose
        {
            get { return lab_close; }
            set { lab_close = value; }
        }
        [DataMember]
        public string LabStatus
        {
            get { return lab_status; }
            set { lab_status = value; }
        }
        [DataMember]
        public int LabMachineCount
        {
            get { return machine_count; }
            set { machine_count = value; }
        }
    }
}
