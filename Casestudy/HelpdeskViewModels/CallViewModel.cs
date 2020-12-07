using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using HelpdeskDAL;

namespace HelpdeskViewModels
{
    public class CallViewModel
    {

        private CallDAO _dao;
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ProblemId { get; set; }
        public string EmployeeName { get; set; }
        public string ProblemDescription { get; set; }
        public string TechName { get; set; }
        public int TechId { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string Notes { get; set; }
        public string Timer { get; set; }


        //constructor
        public CallViewModel()
        {
            _dao = new CallDAO();
        }

        public void GetById()
        {
            try
            {
                Calls cl = _dao.GetById(Id);
             
                Id = cl.Id;
                EmployeeId = cl.EmployeeId;
                ProblemId = cl.ProblemId;
                TechId = cl.TechId;
                DateOpened = cl.DateOpened;
                DateClosed = cl.DateClosed;
                OpenStatus = cl.OpenStatus;
                Notes = cl.Notes;

                Timer = Convert.ToBase64String(cl.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

        }
        public List<CallViewModel> GetAll()
        {
            List<CallViewModel> allVms = new List<CallViewModel>();
            try
            {

                List<Calls> allCalls = _dao.GetAll();

                foreach (Calls cl in allCalls)
                {
                    CallViewModel callVm = new CallViewModel();
                    callVm.Id = cl.Id;
                    callVm.EmployeeId = cl.EmployeeId;
                    callVm.ProblemId = cl.ProblemId;
                    callVm.TechId = cl.TechId;
                    callVm.DateOpened = cl.DateOpened;
                    callVm.DateClosed = cl.DateClosed;
                    callVm.OpenStatus = cl.OpenStatus;
                    callVm.Notes = cl.Notes;
                    callVm.EmployeeName = cl.Employee.LastName;
                    callVm.ProblemDescription = cl.Problem.Description;
                    callVm.TechName = cl.Tech.LastName;
                    callVm.Timer = Convert.ToBase64String(cl.Timer);
                    allVms.Add(callVm);
                }

            }

            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return allVms;
        }

        public void Add()
        {
            Id = -1;
            try
            {
                Calls cl = new Calls
                {

                    EmployeeId =EmployeeId,
                    ProblemId = ProblemId,
                    TechId = TechId,
                    DateOpened =DateOpened,
                    DateClosed = DateClosed,
                    OpenStatus = OpenStatus,
                    Notes = Notes
                };
                Id = _dao.Add(cl);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        public int Update()
        {
            UpdateStatus callUpdated = UpdateStatus.Failed;
            try
            {
                Calls emp = new Calls
                {
                    EmployeeId =EmployeeId,
                    ProblemId = ProblemId,
                    TechId = TechId,
                    DateOpened =DateOpened,
                    DateClosed = DateClosed,
                    OpenStatus = OpenStatus,
                    Id=Id,
                    Notes = Notes
                };

                emp.Timer = Convert.FromBase64String(Timer);
                callUpdated = _dao.Update(emp);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(callUpdated);
        }

        public int Delete()
        {
            int callDeleted = -1;
            try
            {
                callDeleted = _dao.Delete(Id);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return callDeleted;
        }


    }
}
