export const CSS_LANGUAGES = [
  {code: 'en', name: 'English', sidebar_length: 'normal'},
  {code: 'zh-CHS', name: 'Chinese (Simplified)', sidebar_length: 'normal'},
  {code: 'zh-CHT', name: 'Chinese (Traditional)', sidebar_length: 'normal'},
  {code: 'nl', name: 'Dutch', sidebar_length: 'normal'},
  {code: 'de', name: 'Deutsch', length: 'normal'},
  {code: 'fr', name: 'French', sidebar_length: 'long'},
  {code: 'it', name: 'Italian', sidebar_length: 'long'},
  {code: 'ja', name: 'Japanese', sidebar_length: 'long'},
  {code: 'ru', name: 'Russian', sidebar_length: 'long'},
  {code: 'es', name: 'Spanish', sidebar_length: 'long'},
]

export class Language{
    code: string;
    name: string;
    sidebar_length?: string;
  }