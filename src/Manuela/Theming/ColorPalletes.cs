namespace Manuela.Theming;

public static class ColorPalletes
{
    public static Dictionary<UIBrush, Color> BuildDictionary(
        string[] primary,
        string[] secondary,
        string[] tertiary,
        string[] gray,
        int defaultIndex = 6)
    {
        var d = new Dictionary<UIBrush, Color>();

        var swatches = new[]
        {
            UICC.Sw50, UICC.Sw100, UICC.Sw200, UICC.Sw300, UICC.Sw400, UICC.Sw500,
            UICC.Sw600, UICC.Sw700, UICC.Sw800, UICC.Sw900, UICC.Sw950
        };

        var sameLength = primary.Length == secondary.Length &&
            secondary.Length == tertiary.Length &&
            tertiary.Length == gray.Length
            && gray.Length == swatches.Length;

        if (!sameLength)
            throw new ArgumentException("All color palletes length must be 10.");

        for (var i = 0; i < swatches.Length; i++)
        {
            if (i == defaultIndex)
            {
                d[UIBrush.Primary] = Color.FromArgb(primary[i]);
                d[UIBrush.Secondary] = Color.FromArgb(secondary[i]);
                d[UIBrush.Tertiary] = Color.FromArgb(tertiary[i]);
                d[UIBrush.Gray] = Color.FromArgb(gray[i]);
            }

            d[(UIBrush)((int)UIBrush.Primary | swatches[i])] = Color.FromArgb(primary[i]);
            d[(UIBrush)((int)UIBrush.Secondary | swatches[i])] = Color.FromArgb(secondary[i]);
            d[(UIBrush)((int)UIBrush.Tertiary | swatches[i])] = Color.FromArgb(tertiary[i]);
            d[(UIBrush)((int)UIBrush.Gray | swatches[i])] = Color.FromArgb(gray[i]);
        }

        return d;
    }

    public static string[] Blue { get; } =
    [
        "#eff6ff",
        "#dbeafe",
        "#bfdbfe",
        "#93c5fd",
        "#60a5fa",
        "#3b82f6",
        "#2563eb",
        "#1d4ed8",
        "#1e40af",
        "#1e3a8a",
        "#172554"
    ];

    public static string[] Orange { get; } =
    [
        "#fff7ed",
        "#ffedd5",
        "#fed7aa",
        "#fdba74",
        "#fb923c",
        "#f97316",
        "#ea580c",
        "#c2410c",
        "#9a3412",
        "#7c2d12",
        "#431407"
    ];

    public static string[] Green { get; } =
    [
        "#f0fdf4",
        "#dcfce7",
        "#bbf7d0",
        "#86efac",
        "#4ade80",
        "#22c55e",
        "#16a34a",
        "#15803d",
        "#166534",
        "#14532d",
        "#052e16"
    ];

    public static string[] Pink { get; } =
    [
        "#fdf2f8",
        "#fce7f3",
        "#fbcfe8",
        "#f9a8d4",
        "#f472b6",
        "#ec4899",
        "#db2777",
        "#be185d",
        "#9d174d",
        "#831843",
        "#500724"
    ];

    public static string[] Rose { get; } =
    [
        "#fff1f2",
        "#ffe4e6",
        "#fecdd3",
        "#fda4af",
        "#fb7185",
        "#f43f5e",
        "#e11d48",
        "#be123c",
        "#9f1239",
        "#881337",
        "#4c0519"
    ];

    public static string[] Red { get; } =
    [
        "#fef2f2",
        "#fee2e2",
        "#fecaca",
        "#fca5a5",
        "#f87171",
        "#ef4444",
        "#dc2626",
        "#b91c1c",
        "#991b1b",
        "#7f1d1d",
        "#450a0a"
    ];

    public static string[] Slate { get; } =
    [
        "#f8fafc",
        "#f1f5f9",
        "#e2e8f0",
        "#cbd5e1",
        "#94a3b8",
        "#64748b",
        "#475569",
        "#334155",
        "#1e293b",
        "#0f172a",
        "#020617",
    ];

    public static string[] Gray { get; } =
    [
        "#f9fafb",
        "#f3f4f6",
        "#e5e7eb",
        "#d1d5db",
        "#9ca3af",
        "#6b7280",
        "#4b5563",
        "#374151",
        "#1f2937",
        "#111827",
        "#030712"
    ];
}
