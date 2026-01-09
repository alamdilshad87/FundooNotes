namespace DataBaseLayer.Entities
{
    public class NoteLabel
    {
        public int NoteLabelId { get; set; }

        public int NoteId { get; set; }
        public int LabelId { get; set; }

        public Note Note { get; set; } = null!;
        public Label Label { get; set; } = null!;
    }
}