public interface IDoorBehaviour
{
    void InteractStart(Door door);
    void InteractPerform(Door door);
    void InteractCancel(Door door);
}