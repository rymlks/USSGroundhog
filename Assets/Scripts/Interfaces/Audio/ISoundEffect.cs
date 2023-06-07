namespace Interfaces.Audio
{
    public interface ISoundEffect
    {
        public void OnSetSFXVolume()
        {
            SoundEffectPlayer.instance.SetSFXVolume();
        }
    }
}