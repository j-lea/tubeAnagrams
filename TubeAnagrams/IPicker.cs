namespace tubeAnagrams
{
    public interface IPicker<T>
    {
        T Pick(T[] items);
    }
}