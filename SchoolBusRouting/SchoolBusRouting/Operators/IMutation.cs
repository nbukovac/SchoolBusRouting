﻿namespace SchoolBusRouting.Operators
{
    public interface IMutation<T>
    {
        T Mutate(T chromosome);
    }
}