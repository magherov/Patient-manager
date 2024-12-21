import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { enviroment } from '../../enviroments/enviroment';
import { BehaviorSubject, tap } from 'rxjs';

export interface Parameter {
  id: number;
  name: string;
  value: string;
  alarm: boolean;
}

export interface Patient {
  id: number;
  familyName: string;
  givenName: string;
  birthDate: Date;
  sex: string;
  parameters: Array<Parameter>;
}

@Injectable({
  providedIn: 'root',
})
export class PatientService {
  private readonly http = inject(HttpClient);
  private readonly patientPath = '/Patient';
  readonly autorefresh$ = new BehaviorSubject(undefined);

  getPatientList() {
    const headers = new HttpHeaders({
      'Authorization': 'Basic ' + btoa('test:TestMePlease!')
    });

    return this.http.get<Patient[]>(`${enviroment.apiBasePath}${this.patientPath}/GetList`, {
      headers
    });
  }

  updatePatient(patient: Patient) {
    const headers = new HttpHeaders({
      'Authorization': 'Basic ' + btoa('test:TestMePlease!')
    });

    return this.http.post<Patient>(`${enviroment.apiBasePath}${this.patientPath}/Update`, patient, { headers }).pipe(
      tap(() => this.autorefresh$.next(undefined)),
    );
  }
}
