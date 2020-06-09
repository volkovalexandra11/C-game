using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Game.Game_Panels;
using The_Game.Interfaces;

namespace The_Game.Cutscenes
{
    public static class CutsceneFactory
    { 
        public static Dictionary<Cutscene, Func<GameForm, Cutscene, CutscenePanel>> cutsceneBuilder
            = new Dictionary<Cutscene, Func<GameForm, Cutscene, CutscenePanel>>()
            {
                { Cutscene.StartCutscene,  BuildStartCutscene },
                { Cutscene.HideoutCutscene, BuildHideoutCutscene },
                { Cutscene.RebellionOverCutscene, BuildRebellionOverCutscene },
                { Cutscene.CapitalReturnCutscene, BuildCapitalReturnCutscene },
                { Cutscene.FinalCutscene, BuildFinalCutscene },
                { Cutscene.KickTheStumpCutscene, BuildKickTheStumpCutscene }
            };

        public static CutscenePanel BuildCutscene(GameForm form, Cutscene cutscene)
        {
            return cutsceneBuilder[cutscene](form, cutscene);
        }

        private static CutscenePanel BuildStartCutscene(GameForm form, Cutscene cutscene)
        {
            return new CutscenePanel(form, cutscene,
                "CastleCeremony", "StartCutscene");
        }

        private static CutscenePanel BuildKickTheStumpCutscene(GameForm form, Cutscene cutscene)
        {
            return new CutscenePanel(form, cutscene,
                "KickingTheStump", "KickTheStumpCutscene");
        }

        private static CutscenePanel BuildHideoutCutscene(GameForm form, Cutscene cutscene)
        {
            return new CutscenePanel(form, cutscene,
                "Hideout", "HideoutCutscene");
        }

        private static CutscenePanel BuildRebellionOverCutscene(GameForm form, Cutscene cutscene)
        {
            return new CutscenePanel(form, cutscene,
                "RebellionOver", "RebellionOverCutscene");
        }

        private static CutscenePanel BuildCapitalReturnCutscene(GameForm form, Cutscene cutscene)
        {
            return new CutscenePanel(form, cutscene,
                "CastleCeremony", "CapitalReturnCutscene");
        }

        private static CutscenePanel BuildFinalCutscene(GameForm form, Cutscene cutscene)
        {
            return new CutscenePanel(form, cutscene,
                "Final", "FinalCutscene");
        }
    }
}
