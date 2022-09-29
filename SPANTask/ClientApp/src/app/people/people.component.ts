import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalizationService } from '../../services/localization.service'

@Component({
  selector: 'app-people',
  templateUrl: './people.component.html'
})
export class PeopleComponent implements OnInit {
  public people: People[] = [];
  public message?: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private _localizationService: LocalizationService) {
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.http.get<People[]>(this.baseUrl + 'api/people/load-data').subscribe(result => {
      this.people = result;
      this.message = this._localizationService.getLocalizationByMessageId(0);
    }, error => console.error(error));
  }

  saveData() {
    this.http.post<number>(this.baseUrl + 'api/people/save-data', this.people).subscribe(result => {
      this.message = this._localizationService.getLocalizationByMessageId(result);
    });
  }
}

interface People {
  name: string;
  surname: string;
  zipCode: number;
  city: string;
  phoneNumber: string;
  isZipValid: boolean;
}
