namespace AvalonStudio.Debugging.GDB
{
    public class EnablePrettyPrintingCommand : Command<GDBResponse<string>>
    {
        public override int TimeoutMs => DefaultCommandTimeout * 10;

        public override string Encode()
        {
            return "-enable-pretty-printing";
        }

        public override void OutOfBandDataReceived(string data)
        {
            
        }

        protected override GDBResponse<string> Decode(string response)
        {
            return new GDBResponse<string>(DecodeResponseCode(response));
        }
    }
}
