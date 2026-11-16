namespace ForeignCard.Common.Exceptions;

public class AccountLockedException : Exception
{
    public AccountLockedException(int minutesLeft): base($"Cok fazla hatali deneme. {minutesLeft} dakika sonra tekrar deneyin.")
    {
    }
}