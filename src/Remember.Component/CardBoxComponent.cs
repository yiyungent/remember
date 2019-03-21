﻿using Remember.Domain;
using Remember.Manager;
using Remember.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remember.Component
{
    public class CardBoxComponent : BaseComponent<CardBox, CardBoxManager>, CardBoxService
    {
    }
}
