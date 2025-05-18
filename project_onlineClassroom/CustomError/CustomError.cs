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

}