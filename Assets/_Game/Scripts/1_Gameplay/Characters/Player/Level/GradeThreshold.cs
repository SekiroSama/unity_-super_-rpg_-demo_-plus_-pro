public enum StyleGrade { D, C, B, A, S, SS, SSS }

[System.Serializable]
public class GradeThreshold
{
    public StyleGrade grade;
    public float minScore;
}