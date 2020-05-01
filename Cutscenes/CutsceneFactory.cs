using System;
using System.Collections.Generic;
using The_Game.Game_Panels;

namespace The_Game.Cutscenes
{
    public static class CutsceneFactory
    { 
        public static Dictionary<Cutscene, Func<GameForm, Cutscene, CutscenePanel>> cutsceneBuilder
            = new Dictionary<Cutscene, Func<GameForm, Cutscene, CutscenePanel>>()
            {
                { Cutscene.StartCutscene,  BuildStartCutscene },
                { Cutscene.KickTheStumpCutscene, BuildKickTheStumpCutscene }
            };

        public static CutscenePanel BuildCutscene(GameForm form, Cutscene cutscene)
        {
            return cutsceneBuilder[cutscene](form, cutscene);
        }

        private static CutscenePanel BuildStartCutscene(GameForm form, Cutscene cutscene)
        {
            return new CutscenePanel(form, cutscene,
                "CastleCeremony.jpg", "StartCutscene.txt");
        }

        private static CutscenePanel BuildKickTheStumpCutscene(GameForm form, Cutscene cutscene)
        {
            return new CutscenePanel(form, cutscene,
                "KickingTheStump.png", "KickTheStumpCutscene.txt");
        }
    }
}
