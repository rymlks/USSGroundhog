namespace EasyState.Builders
{
    public abstract class Builder<TItem>
    {
        public abstract TItem Build();
        public static implicit operator TItem(Builder<TItem> builder)=> builder.Build();
    }

}