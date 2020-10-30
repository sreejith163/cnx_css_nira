export interface CompressedIconData {
    name: string;
    unified: string;
    shortName: string;
    shortNames?: string[];
    sheet: [number, number];
    keywords?: string[];
    hidden?: string[];
    emoticons?: string[];
    text?: string;
    skinVariations?: IconVariation[];
    obsoletedBy?: string;
    obsoletes?: string;
}

export interface IconVariation {
    unified: string;
    sheet: [number, number];
    hidden?: string[];
}
