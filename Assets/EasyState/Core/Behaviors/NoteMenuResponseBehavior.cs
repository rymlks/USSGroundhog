using EasyState.Core.Models;
using System;
using System.Linq;
using UnityEngine;

namespace EasyState.Core.Behaviors
{
    internal class NoteMenuResponseBehavior : Behavior
    {
        public NoteMenuResponseBehavior(Design design) : base(design)
        {
            design.ContextMenuResponse += Design_ContextMenuResponse;
        }

        public override void Dispose()
        {
            Design.ContextMenuResponse -= Design_ContextMenuResponse;
        }

        private void Design_ContextMenuResponse(ContextMenuResponse responseType, Vector2 point, string data)
        {
            if (responseType == ContextMenuResponse.CreateNote)
            {
                var position = Design.GetRelativePosition(point) - Design.ToolbarOffset;

                var newNote = new Note(position);
                Design.Notes.Add(newNote);
            }
            else if (responseType == ContextMenuResponse.DeleteNote)
            {
                var selectedModels = Design.GetSelectedModels().ToList();
                foreach (var model in selectedModels)
                {
                    if(model is Note)
                    {
                        var note = (Note)model;
                        Design.Notes.Remove(note);
                        if(note.Group != null)
                        {
                            note.Group.RemoveChild(note);
                        }
                    }
                }                
            }
        }
    }
}