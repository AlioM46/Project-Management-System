namespace Project_Management_System
{
    public class enums

    {

        public enum enTasksPriority
        {
            Low, Medium, High
        }

        public enum enNotificationsTypes
        {
            TaskAssigned, TaskUpdated, DeadlineReminder, NotSpecefied
        }

        public enum enRoles
        {
            Employee = 1,
            ProjectManager,
            Admin
        }

    
    

        public enum enProjectStatus
        {
            InProgress,
            Cancelled,
            Completed
        }

        public enum enTaskStatus
        {

            InProgress,
            Cancelled,
            Completed
        }
        public enum enApplicationStatus
        {
            Pending,
            Accepted,
            Rejected
        }


    }
}
