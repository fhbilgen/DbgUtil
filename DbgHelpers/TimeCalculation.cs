namespace DbgHelpers
{
    public class TimeCalculation
    {

        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }        
        public int CPUs { get; set; }

        public TimeCalculation()
        {
            Days = 0;
            Hours = 0;
            Minutes = 0;
            Seconds = 0;
            CPUs = 1;
        }
        public TimeCalculation(int hours, int minutes, int seconds, int cpus = 1, int days = 0)
        {
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            CPUs = cpus;
        }
        public int TotalMinutes
        {
            get
            {
                return CPUs * (((((Days * 24) + Hours) * 60) + Minutes));
            }
        }

        public int TotalSeconds
        {
            get
            {
                return CPUs * ((((((Days * 24) + Hours) * 60) + Minutes)*60) + Seconds);
            }
        }

        public static int DifferenceMinutes(TimeCalculation t1, TimeCalculation t2)
        {
            var Result = t1.TotalMinutes - t2.TotalMinutes;

            if (Result < 0)
                return Result * -1;
            else
                return Result;
        }

        public static int DifferenceSeconds(TimeCalculation t1, TimeCalculation t2)
        {
            var Result = t1.TotalSeconds - t2.TotalSeconds;

            if (Result < 0)
                return Result * -1;
            else
                return Result;
        }

        public static int SumMinutes(TimeCalculation t1, TimeCalculation t2)
        {
            return t1.TotalMinutes + t2.TotalMinutes;
        }

        public static int SumSeconds(TimeCalculation t1, TimeCalculation t2)
        {
            return t1.TotalSeconds + t2.TotalSeconds;
        }


    }
}
