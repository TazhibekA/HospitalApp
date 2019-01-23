using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    class Program
    {
        //(LocalDb)\MSSQLLocalDB
        static void Main(string[] args)
        {
            HospitalAppointments hospital = new HospitalAppointments();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("1 - Записаться к врачу");
                Console.WriteLine("2 - Посмотреть расписание докторов");
                Console.WriteLine("3 - Выход");
                Console.Write("Введите: ");

                string choose = Console.ReadLine();
                int chooseInt;

                bool success = Int32.TryParse(choose, out chooseInt);

                if (success && chooseInt <= 3 && chooseInt > 0)
                {
                    switch (chooseInt) {
                        case 1:

                            AddAppointment(hospital);
                            Console.Read();
                            break;
                        case 2:
                            Console.Clear();
                            bool anydoctor = hospital.Doctors.Any();
                            if (anydoctor)
                            {
                                var doctors = from doc in hospital.Doctors
                                              join dep in hospital.Departments
                                              on doc.DepartmentId equals dep.Id
                                              join sch in hospital.ScheduleDoctors
                                              on doc.Id equals sch.DoctorId
                                              select new
                                              {
                                                  Id = doc.Id,
                                                  FullName = doc.FullName,
                                                  DepartmentName = dep.Name,
                                                  StartJob = sch.StartTime,
                                                  FinishJob = sch.FinishTime
                                              };
                                Console.WriteLine("ФИО      Отделение     Рабочее время(будние дни)");
                                foreach (var doctor in doctors)
                                {
                                    Console.WriteLine("{0}      {1}     {2}-{3}", doctor.FullName, doctor.DepartmentName, doctor.StartJob, doctor.FinishJob);
                                }
                            }
                            else {
                                Console.WriteLine("Нет врачей!");

                            }
                            Console.Read();
                            break;
                        case 3:
                            Environment.Exit(0);
                            break;

                    }
                }
                else
                {
                    Console.WriteLine("Enter 1-3!");

                }
               
            }
        }

        public static void AddAppointment(HospitalAppointments hospital) {

            Console.Clear();
            bool anydoctor = hospital.Doctors.Any();
            if (anydoctor)
            {
                Console.Write("Введите ФИО: ");
                string fullName = Console.ReadLine();
                var doctors = from doc in hospital.Doctors
                              join dep in hospital.Departments
                              on doc.DepartmentId equals dep.Id
                              select new
                              {
                                  Id = doc.Id,
                                  FullName = doc.FullName,
                                  DepartmentName = dep.Name
                              };
                Console.WriteLine("ID   ФИО     Отделение");
                foreach (var doctor in doctors)
                {
                    Console.WriteLine("{0}. {1}     {2}", doctor.Id, doctor.FullName, doctor.DepartmentName);
                }
                Console.Write("Выберите ID врача: ");
                string idDoctor = Console.ReadLine();
                int intIdDoctor;
                bool successParseIdDoctor = int.TryParse(idDoctor, out intIdDoctor);

                if (!successParseIdDoctor)
                {
                    Console.WriteLine("Enter 1 - {0}!", doctors.ToList().Count);
                    Console.Read();
                    return;
                }
                var schedule = from sch in hospital.ScheduleDoctors
                               join doc in hospital.Doctors
                              on sch.DoctorId equals doc.Id
                               where doc.Id == intIdDoctor
                               select sch;


                Console.Write("Рабочее время врача (только будние дни): ");
                int startTimeHours = 0;
                int startTimeMinutes = 0;
                int finishTimeHours = 0;
                int finishTimeMinutes = 0;
                foreach (var sch in schedule)
                {
                    Console.WriteLine("{0} - {1} ", sch.StartTime, sch.FinishTime);
                    startTimeHours = sch.StartTime.Hours;
                    startTimeMinutes = sch.StartTime.Minutes;
                    finishTimeHours = sch.FinishTime.Hours;
                    finishTimeMinutes = sch.FinishTime.Minutes;
                }
                bool correctTime = false;
                while (!correctTime)
                {
                    Console.WriteLine("Введите месяц: ");
                    string month = Console.ReadLine();
                    int monthInt;
                    bool successIntMonth = int.TryParse(month, out monthInt);

                    Console.WriteLine("Введите день: ");
                    string day = Console.ReadLine();
                    int dayInt;
                    bool successIntDay = int.TryParse(day, out dayInt);

                    Console.WriteLine("Введите часы: ");
                    string hours = Console.ReadLine();
                    int hoursInt;
                    bool successIntHours = int.TryParse(hours, out hoursInt);

                    Console.WriteLine("Введите минуты: ");
                    string minutes = Console.ReadLine();
                    int minutesInt;
                    bool successIntMinutes = int.TryParse(minutes, out minutesInt);

                    if (successIntMonth && successIntDay && monthInt <= 12 && monthInt >= 1 && dayInt <= 31 && dayInt >= 1)
                    {
                        DateTime dateTime = new DateTime(2019, monthInt, dayInt);

                        if (successIntHours && successIntMinutes && hoursInt >= startTimeHours && hoursInt <= finishTimeHours
                            && minutesInt <= 60 && minutesInt >= 0 && dateTime.DayOfWeek != DayOfWeek.Sunday && dateTime.DayOfWeek != DayOfWeek.Saturday)
                        {
                            DateTime correctDateTime = new DateTime(2019, monthInt, dayInt, hoursInt, minutesInt, 0);

                            bool times = hospital.Appointments.Any(s => s.Time == correctDateTime);
                            if (!times)
                            {
                                Appointment appointment = new Appointment
                                {
                                    ClientFullName = fullName,
                                    Time = correctDateTime,
                                    DoctorId = intIdDoctor
                                };
                                hospital.Appointments.Add(appointment);
                                hospital.SaveChanges();
                                Console.WriteLine("Вы записаны!");
                                correctTime = true;
                            }
                            else
                            {
                                Console.WriteLine("В это время уже занято! Выберите другое время");
                            }


                        }
                        else
                        {
                            Console.WriteLine("Введите правильное время(только будние дни)!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Введите правильный месяц и день(будний)!");
                    }
                }
            }
            else {
                Console.WriteLine("Нет врачей!");
                
            }
        }
    }
}
