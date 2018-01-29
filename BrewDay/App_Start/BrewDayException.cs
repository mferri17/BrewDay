using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BrewDay
{
    public class BrewDayException : Exception
    {
        public BrewDayException() : base("Qualcosa è andato storto. Assicurati che l'operazione che hai tentato di portare a termine sia fattibile.") { }
        public BrewDayException(string message) : base(message) { }
        public BrewDayException(string message, Exception innerException) : base(message, innerException) { }
        protected BrewDayException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public class MissingIdBrewDayException : BrewDayException
    {
        public MissingIdBrewDayException() : base("Id dell'elemento non specificato. Controlla che l'url sia corretto.") { }
        public MissingIdBrewDayException(string message) : base(message) { }
        public MissingIdBrewDayException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InvalidIdBrewDayException : BrewDayException
    {
        public InvalidIdBrewDayException() : base("Id specificato non valido. Controlla che l'url sia corretto.") { }
        public InvalidIdBrewDayException(int id) : base($"Id {id} non valido. Controlla che l'url sia corretto.") { }
        public InvalidIdBrewDayException(string message) : base(message) { }
        public InvalidIdBrewDayException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InvalidOperationBrewDayException : BrewDayException
    {
        public InvalidOperationBrewDayException() : base("Non è possibile effettuare questa operazione poiché mancano delle condizioni sufficienti affinché possa essere eseguita.") { }
        public InvalidOperationBrewDayException(string message) : base(message) { }
        public InvalidOperationBrewDayException(string message, Exception innerException) : base(message, innerException) { }
    }
}