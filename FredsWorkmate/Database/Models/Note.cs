using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FredsWorkmate.Database.Models
{
    public class Note : Model
    {
        public required string Title { get; set; }
        public required string Content { get; set; }

        public string? OwnerId { get; set; }
        public string? OwnerType { get; set; }
    }

    public static class NoteExtensions
    {
        public static void AddNote<T>(this T owner, Note note) where T : class, INoteOwner
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (note == null) throw new ArgumentNullException(nameof(note));

            note.SetOwner(owner);
            owner.Notes.Add(note);
        }

        public static INoteOwner? GetOwner(this Note note, DatabaseContext dbContext)
        {
            if (note.OwnerType == null || note.OwnerId == null)
            {
                return null;
            }

            var ownerType = Type.GetType(note.OwnerType) ?? throw new InvalidOperationException($"Could not find type {note.OwnerType}");

            object ownerSet = dbContext.GetEntityDbSet(ownerType);

            var setType = ownerSet.GetType();
            var method = setType.GetMethod("Find") ?? throw new InvalidOperationException($"Could not find Find method on set {ownerSet}");
            return (INoteOwner?)method.Invoke(ownerSet, [new object[] { note.OwnerId }]);
        }

        public static void SetOwner(this Note note, INoteOwner? owner)
        {
            if (owner == null)
            {
                note.OwnerId = null;
                note.OwnerType = null;
            }
            else
            {
                note.OwnerId = owner.Id;
                note.OwnerType = owner.GetType().FullName;
            }
        }
    }
}
