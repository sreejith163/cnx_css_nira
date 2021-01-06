export const CSS_LANGUAGES = [
  { code: 'en', name: 'English', sidebarLength: 'normal' },
  { code: 'zh-CHS', name: 'Chinese (Simplified)', sidebarLength: 'normal' },
  { code: 'zh-CHT', name: 'Chinese (Traditional)', sidebarLength: 'normal' },
  { code: 'nl', name: 'Dutch', sidebarLength: 'normal' },
  { code: 'de', name: 'Deutsch', length: 'normal' },
  { code: 'fr', name: 'French', sidebarLength: 'long' },
  { code: 'it', name: 'Italian', sidebarLength: 'long' },
  { code: 'ja', name: 'Japanese', sidebarLength: 'long' },
  { code: 'ru', name: 'Russian', sidebarLength: 'long' },
  { code: 'es', name: 'Spanish', sidebarLength: 'long' },
];

export class Language {
  code: string;
  name: string;
  sidebarLength?: string;
}
