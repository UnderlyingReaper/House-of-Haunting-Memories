public interface IPlayerSpeak
{
    public enum SpeechType {
        Main,
        Hint,
        Custom
    }

    public void SpeakPlayer(SpeechType type, PlayerSpeechData playerSpeechData = null);
}