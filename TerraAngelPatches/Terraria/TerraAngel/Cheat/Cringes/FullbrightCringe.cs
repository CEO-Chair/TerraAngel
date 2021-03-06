using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace TerraAngel.Cheat.Cringes
{
    public class FullBrightCringe : Cringe
    {
        public override string Name => "Fullbright";

        public override CringeTabs Tab => CringeTabs.LightingCheats;

        public float Brightness = 0.8f;

        public override void DrawUI(ImGuiIOPtr io)
        {
            base.DrawUI(io);

            ImGui.TextUnformatted("Brightness"); ImGui.SameLine();
            float tmp = Brightness * 100f;
            if (ImGui.SliderFloat("##Brightness", ref tmp, 1f, 100f))
            {
                Brightness = tmp / 100f;
            }
        }
    }
}
