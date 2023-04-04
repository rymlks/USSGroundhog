using EasyState.DataModels;
using System;
using UnityEngine;

namespace EasyState.Core.Models
{
    public class Note : MoveableModel, IGroupable, IDuplicable, IDataModel<NoteData>
    {
        public string Contents { get; set; }
        public Group Group { get; set; }
        public bool CanDuplicate => Group == null || !(Group?.Selected).GetValueOrDefault();
        public Color NoteColor { get; set; } = new Color(0.7372549f, 0.7254902f, 0.3529412f);
        public Color TextColor { get; set; } = new Color(0.2941177f, 0.2862745f, 0.2862745f);
        public const string DEFAULT_NOTE = "Double click to edit";
        public const string DEFAULT_TITLE = "New Note";
        public Rect Rect
        {
            get
            {
                _rect.position = Position;
                return _rect;
            }
            set
            {
                if (_rect != value)
                {
                    _rect.width = value.width;
                    _rect.height = value.height;
                }
            }
        }

        public Color SelectedNodeColor { get; set; }
        private Rect _rect;
        public Note(NoteData data) : base(data)
        {
            Contents = data.Contents;
            NoteColor = data.NoteColor;
            TextColor = data.TextColor;            
        }

        public Note(Vector2? position = null, string id = null) : base(position, id)
        {
        }
        public void OnDelete(Design design) => design.Notes.Remove(this);

        public void OnGroupRefresh() => Refresh();

        public IDuplicable Duplicate(Design design)
        {
            var noteData = Serialize();
            noteData.Id = Guid.NewGuid().ToString();
            noteData.Position += new Vector2(30, 30);
            var duplicatedNote = new Note(noteData);
            design.Notes.Add(duplicatedNote);
            if(Group != null)
            {
                duplicatedNote.Group = Group;
                Group.AddChild(duplicatedNote);
            }
            return duplicatedNote;
        }

        public NoteData Serialize()
        {
            var data = new NoteData()
            {

                Contents = Contents,
                NoteColor = NoteColor,
                TextColor = TextColor,

            };
            data.SetModelData(this);
            return data;
        }
    }
}