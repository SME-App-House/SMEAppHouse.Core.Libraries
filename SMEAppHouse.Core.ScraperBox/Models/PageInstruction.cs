namespace SMEAppHouse.Core.ScraperBox.Models
{
    /// <summary>
    /// '{0-2-1}' means : '0' character at '2' digits pad to left(0)/right(1)
    /// </summary>
    public class PageInstruction
    {
        public enum PaddingDirectionsEnum
        {
            ToLeft = 0,
            ToRight = 1
        }

        public char PadCharacter { get; set; }
        public int PadLength { get; set; }
        public PaddingDirectionsEnum PaddingDirection { get; set; }
    }
}
