namespace PilotLookUp.Domain.Entities
{
    public class AttrDTO
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string IsObligatory { get; set; }
        public string IsService { get; set; }
        public string Type { get; set; }
        public bool IsInitialized { get; set; }
        public bool IsValid { get; set; }
    }
}
