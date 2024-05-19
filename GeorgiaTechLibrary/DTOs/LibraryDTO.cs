namespace GeorgiaTechLibrary.DTOs
{
    public class LibraryDTO
    {
        public string Name { get; set; }
        public List<UserDTO> Users { get; set; }
        public AddressDTO LibraryAddress { get; set; }
        public List<BookInstanceDTO> BookInstances { get; set; }
    }
}
