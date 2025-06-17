namespace project_onlineClassroom.CustomError
{
    public class UserExistsException : Exception
    {
        public UserExistsException() : base("User already exists.")
        {
        }
        public UserExistsException(string message) : base(message)
        {
        }
        public UserExistsException(string message, Exception inner) : base(message, inner)
        {
        }

    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found.")
        {
        }
        public UserNotFoundException(string message) : base(message)
        {
        }
        public UserNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base("Invalid password.")
        {
        }
        public InvalidPasswordException(string message) : base(message)
        {
        }
        public InvalidPasswordException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException() : base("Email is invalid. Please correct.")
        {
        }
        public InvalidEmailException(string message) : base(message)
        {
        }
        public InvalidEmailException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    public class PasswordMismatchException : Exception
    {
        public PasswordMismatchException() : base("Passwords do not match.")
        {
        }
        public PasswordMismatchException(string message) : base(message)
        {
        }
        public PasswordMismatchException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() : base("Invalid token.")
        {
        }
        public InvalidTokenException(string message) : base(message)
        {
        }
        public InvalidTokenException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    public class ClassNotFoundException : Exception
    {
        public ClassNotFoundException() : base("Class not found.")
        {
        }
        public ClassNotFoundException(string message) : base(message)
        {
        }
        public ClassNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    public class ClassFullException : Exception
    {
        public ClassFullException() : base("Class is full.")
        {
        }
        public ClassFullException(string message) : base(message)
        {
        }
        public ClassFullException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    public class RoleMismatchForThisActionException : Exception
    {
        public RoleMismatchForThisActionException() : base("Role mismatch for this action.")
        {
        }
        public RoleMismatchForThisActionException(string message) : base(message)
        {
        }
        public RoleMismatchForThisActionException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    public class ClassAlreadyExistsException : Exception
    {
        public ClassAlreadyExistsException() : base("Class already exists.")
        {
        }
        public ClassAlreadyExistsException(string message) : base(message)
        {
        }
        public ClassAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    public class AlreadyParticipantException : Exception
    {
        public AlreadyParticipantException() : base("User is already a participant in this class.")
        {
        }
        public AlreadyParticipantException(string message) : base(message)
        {
        }
        public AlreadyParticipantException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    public class DataValidationException : Exception
    {
        public DataValidationException() : base("Data validation failed. Please correct your inputs.")
        {
        }
        public DataValidationException(string message) : base(message)
        {
        }
        public DataValidationException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    public class NotParticipantException : Exception
    {
        public NotParticipantException() : base("User is not a participant in this class.")
        {
        }
        public NotParticipantException(string message) : base(message)
        {
        }
        public NotParticipantException(string message, Exception inner) : base(message, inner)
        {
        }
    }

}