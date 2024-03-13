using CoreWCF;

namespace PhraseFinder.WCF.Services;

public class EchoService : IEchoService
{
    public string Echo(string text)
    {
        Console.WriteLine($"Received {text} from client");
        return text;
    }

    public string ComplexEcho(EchoMessage message)
    {
        Console.WriteLine($"Received {message.Text} from client");
        return message.Text;
    }

    public string FailEcho(string text) => 
        throw new FaultException<EchoFault>(
            new EchoFault
            {
                Text = "This is a failure"
            },
            new FaultReason("FailReason"));
}