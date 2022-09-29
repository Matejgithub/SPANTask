import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalizationService {

  constructor() { }

  getLocalizationByMessageId(id: number) {
    switch (id) {
      case 0:
        return "Podaci učitani";
      case 1:
        return "Nisu spremljeni svi podaci"
      case 2:
        return "Nije spremljen niti jedan podatak (ili već postoje ili su poštanski brojevi neispravni)"
      case 3:
        return "Svi podaci spremljeni"
      default:
        return "Neočekivana greška"
    }
  }
}
