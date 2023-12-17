using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SIUCMachineAvailabilityService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SIUCMacineAvailabilityService : IService1
    {
        public bool Initialize(string machine_name, string mac_address, string ip_address)
        {
            bool success = false;
            using (machinesEntities MachineData = new machinesEntities())
            {
                var query = from m in MachineData.Machines
                            where m.mname == machine_name
                            select m;

                DateTime now = DateTime.Now;
                success = InsertEvent(query.FirstOrDefault().mid, "initialized");
                query.FirstOrDefault().mmac = mac_address;
                query.FirstOrDefault().mlkip = ip_address;
                // Submit the changes to the database.
                try
                {
                    MachineData.SaveChanges();
                    success = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                    success = false;
                }
            }
            return success;
        }

        public bool Login(string machine_name)
        {
            bool success = false;
            using (machinesEntities MachineData = new machinesEntities()) {
                var query = from a in MachineData.Availabilities
                            join m in MachineData.Machines
                            on a.fmid equals m.mid
                            where m.mname == machine_name
                            select a;

                DateTime now = DateTime.Now;
                if ((now - query.FirstOrDefault().adate).TotalSeconds > 5) {
                    success = InsertEvent(query.FirstOrDefault().fmid, "login");
                    query.FirstOrDefault().mavailable = false;
                }
                query.FirstOrDefault().adate = now;
                // Submit the changes to the database.
                try {
                    MachineData.SaveChanges();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                    success = false;
                }
            }
            return success;
        }

        public bool Logout(string machine_name)
        {
            bool success = false;
            using (machinesEntities MachineData = new machinesEntities()) {
                var query = from a in MachineData.Availabilities
                            join m in MachineData.Machines
                            on a.fmid equals m.mid
                            where m.mname == machine_name
                            select a;

                DateTime now = DateTime.Now;
                if ((now - query.FirstOrDefault().adate).TotalSeconds > 5) {
                    success = InsertEvent(query.FirstOrDefault().fmid, "logout");
                    query.FirstOrDefault().mavailable = true;
                }
                query.FirstOrDefault().adate = now;
                // Submit the changes to the database.
                try {
                    MachineData.SaveChanges();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                    success = false;
                }
            }
            return success;
        }

        public bool Reset(string machine_name) {
            bool success = false;
            using (machinesEntities MachineData = new machinesEntities()) {
                var query = from a in MachineData.Availabilities
                            join m in MachineData.Machines
                            on a.fmid equals m.mid
                            where m.mname == machine_name
                            select a;

                DateTime now = DateTime.Now;
                if ((now - query.FirstOrDefault().adate).TotalSeconds > 5) {
                    success = InsertEvent(query.FirstOrDefault().fmid, "reset");
                    query.FirstOrDefault().mavailable = true;
                }
                query.FirstOrDefault().adate = now;
                // Submit the changes to the database.
                try {
                    MachineData.SaveChanges();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                    success = false;
                }
            }
            return success;
        }

        public bool InsertEvent(int fmid, string estring)
        {
            machinesEntities MachineData = new machinesEntities();

            Event NewEvent = new Event();
            
            NewEvent.fmid = fmid;
            NewEvent.edate = DateTime.Now;
            NewEvent.estring = estring;

            MachineData.Events.Add(NewEvent);

            // Submit the changes to the database.
            try
            {
                MachineData.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Provide for exceptions.
            }
            return false;
        }

        public List<LabObject> GetAllLabs()
        {
            var labs = new List<LabObject>();
            
            using (machinesEntities MachineData = new machinesEntities())
            {
                var query = from l in MachineData.Labs
                            select l;

                foreach (var lab in query)
                {
                    LabObject new_lab = new LabObject();

                    new_lab.LabId = lab.lid;

                    new_lab.LabName = lab.lname;
                    new_lab.LabBuilding = lab.lbuilding;
                    new_lab.LabFloor = (int)lab.lfloor;
                    new_lab.LabRoom = lab.lroom;
                    new_lab.LabOpen = lab.lopen.ToString();
                    new_lab.LabClose = lab.lclose.ToString();
                    new_lab.LabStatus = lab.lstat;
                    new_lab.LabMachineCount = lab.mcount;
                    
                    labs.Add(new_lab);
                }
            }

            return labs;
        }

        public List<MachineObject> GetComputersInLab(string lab_name)
        {
            var computers = new List<MachineObject>();

            using (machinesEntities MachineData = new machinesEntities())
            {
                var query = from m in MachineData.Machines
                            join l in MachineData.Labs
                            on m.flid equals l.lid
                            where l.lname == lab_name
                            select m;

                foreach (var machine in query)
                {
                    MachineObject new_machine = new MachineObject();

                    new_machine.MachineId = machine.mid;
                    new_machine.MachineName = machine.mname;
                    new_machine.MachineMac = machine.mmac;
                    new_machine.MachineIP = machine.mlkip;
                    new_machine.MachineLabel = machine.mlabel;
                    new_machine.MachineType = machine.mtype;

                    computers.Add(new_machine);
                }
            }

            return computers;
        }

        public List<string> GetComputersStatus(string lab_name)
        {
            var StatusList = new List<string>();

            using (machinesEntities MachineData = new machinesEntities())
            {
                var query = from m in MachineData.Machines
                            join l in MachineData.Labs
                            on m.flid equals l.lid
                            where l.lname == lab_name
                            select m;

                foreach (var M in query)
                {
                    StatusList.Add(M.mstat);
                }

                return StatusList;
            }
        }

        public List<bool> GetComputersAvailability(string lab_name)
        {
            var AvailabilityList = new List<bool>();

            using (machinesEntities MachineData = new machinesEntities())
            {
                var query = from a in MachineData.Availabilities
                            join m in MachineData.Machines
                            on a.fmid equals m.mid
                            join l in MachineData.Labs
                            on m.flid equals l.lid
                            where l.lname == lab_name
                            select a;

                foreach (var A in query)
                {                    
                    AvailabilityList.Add(A.mavailable);
                }

                return AvailabilityList;
            }
        }

        public bool GetLabAvailability(string lab_name)
        {
            using (machinesEntities MachineData = new machinesEntities())
            {
                var query = from l in MachineData.Labs
                            where l.lname == lab_name
                            select l;

                foreach (var A in query)
                {
                    if (A.lstat == "Open")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
        }
    }
}