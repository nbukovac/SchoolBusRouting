﻿using System.Collections.Generic;
using SchoolBusRouting.Models;

namespace SchoolBusRouting.Operators
{
    public interface ICrossover<T>
    {
        T Cross(T chromosome1, T chromosome2);
    }
}