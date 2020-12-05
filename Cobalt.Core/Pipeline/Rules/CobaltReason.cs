namespace Cobalt.Pipeline.Rules
{
    public class CobaltReason
    {
        public CobaltReason(bool isSatisfied, string message)
        {
            IsSatisfied = isSatisfied;
            Message = message;
        }

        public bool IsSatisfied { get; set; }
        public string Message { get; set; }
    }
}