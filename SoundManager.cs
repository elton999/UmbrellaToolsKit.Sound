using FMOD.Studio;
using System;

namespace UmbrellaToolsKit.Sound
{
    public class SoundManager
    {
        public FMOD.Studio.System SoundSystem;
        public FMOD.Studio.Bank LocalizedBank;

        public static SoundManager Instance => _instance;

        private static SoundManager _instance;

        public SoundManager(string bankPath)
        {
            if (_instance != null) return;

            _instance = this;

            var result = FMOD.Studio.System.create(out SoundSystem);

            if (!LogResult(result)) return;

            result = SoundSystem.initialize(512, FMOD.Studio.INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, new IntPtr(0));

            if (!LogResult(result)) return;

            result = SoundSystem.loadBankFile(System.IO.Path.GetFullPath(bankPath), FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out LocalizedBank);

            if (!LogResult(result)) return;
        }

        public void Update() => SoundSystem.update();

        public void Dispose()
        {
            SoundSystem.release();
            LocalizedBank.unload();
        }

        public EventInstance GetEventInstance(FMOD.GUID id)
        {
            EventDescription eventDescription;
            EventInstance eventInstance;

            if (!LogResult(SoundSystem.getEventByID(id, out eventDescription))) return default;

            if (!LogResult(eventDescription.createInstance(out eventInstance))) return default;

            return eventInstance;
        }

        public static bool LogResult(FMOD.RESULT result)
        {
            if (result != FMOD.RESULT.OK)
            {
                Console.WriteLine($"FMOD error! {result} {FMOD.Error.String(result)}\n");
                return false;
            }
            return true;
        }
    }
}
