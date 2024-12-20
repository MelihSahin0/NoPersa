namespace SharedLibrary.Util
{
    public class IdGenerator
    {
        private int Id; 

        public IdGenerator(int startId)
        {
            Id = startId;
        }

        public int GetId()
        {
            return Id++;
        }
    }
}
