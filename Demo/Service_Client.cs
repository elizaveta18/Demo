//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Demo
{
    using System;
    using System.Collections.Generic;
    
    public partial class Service_Client
    {
        public int Id { get; set; }
        public Nullable<int> Id_service { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> Id_client { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual Service Service { get; set; }
    }
}
