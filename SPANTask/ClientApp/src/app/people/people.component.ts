import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-people',
  templateUrl: './people.component.html'
})
export class PeopleComponent implements OnInit {
  public people: People[] = [];
  public message?: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.http.get<People[]>(this.baseUrl + 'api/people/load-data').subscribe(result => {
      this.people = result;
      this.message = "Uspješno učitani podaci";
    }, error => console.error(error));
  }

  saveData() {
    this.http.post<number>(this.baseUrl + 'api/people/save-data', this.people).subscribe(result => {
      this.message = this.getMessageById(result);
    });
  }

  getMessageById(id: number) {
    console.log(id);
    var message = "";
    if (id == 1) {
      message = "Nisu spremljani svi podaci!"
    }
    if (id == 2) {
      message = "Nisu spremljani podaci!"
    }
    if (id == 3) {
      message = "Spremljeni su svi podaci!"
    }
    return message;
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
