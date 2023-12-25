namespace Flowframes.Data
{
    public class EncoderInfo
    {
        public string Name { get; set; } = "unknown";

        public EncoderInfo() { }

        public EncoderInfo(string name)
        {
            Name = name;
        }
    }
}
