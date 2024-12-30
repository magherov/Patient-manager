import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { enviroment } from '../../enviroments/enviroment';
import { BehaviorSubject, tap } from 'rxjs';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { MessageService } from 'primeng/api';
import { AuthService } from './auth.service';

export interface FilterPatientInput {
  givenName?: string | null;
  familyName?: string | null;
  SortBy?: string | null;
  SortDirection?: 'asc' | 'desc' | null;
}

export interface Parameter {
  id?: number;
  name?: string;
  value?: string;
  alarm?: boolean;
}

export interface Patient {
  id?: number;
  familyName?: string;
  givenName?: string;
  birthDate?: Date;
  sex?: string;
  parameters?: Array<Parameter> | null;
}

export interface PatientExtend {
  patient?: Patient;
  action: 'update' | 'create';
}

@Injectable({
  providedIn: 'root',
})
export class PatientService {
  private readonly http = inject(HttpClient);
  private readonly toastService = inject(MessageService);
  private readonly authService = inject(AuthService);
  private readonly patientPath = '/Patient';
  private readonly hubConnection = new HubConnectionBuilder().withUrl(`${enviroment.basePath}/alarmHub`).build();

  readonly autorefresh$ = new BehaviorSubject<void>(undefined);

  constructor() {
    if (this.authService.getToken()) {
      this.hubConnection.start();
      this.hubConnection.on('PatientsUpdated', () => { 
        this.autorefresh$.next();
        this.toastService.add({
          severity: 'success',
          summary: 'Pazienti aggiornati',
          closable: true,
        });
      });
    }
  }

  getPatientList(filter?: Partial<FilterPatientInput>) {
    const params = new HttpParams()
      .set('GivenName', filter?.givenName ?? '')
      .set('FamilyName', filter?.familyName ?? '')
      .set('SortBy', filter?.SortBy ?? '')
      .set('SortDirection', filter?.SortDirection ?? '');
    
    return this.http.get<Patient[]>(`${enviroment.apiBasePath}${this.patientPath}`, { params });
  }

  updatePatient(patient: Patient) {
    return this.http.put<Patient>(`${enviroment.apiBasePath}${this.patientPath}/${patient.id}`, patient).pipe(
      tap(() => this.autorefresh$.next()),
    );
  }

  deletePatient(id: number) {
    return this.http.delete(`${enviroment.apiBasePath}${this.patientPath}/${id}`).pipe(
      tap(() => this.autorefresh$.next()),
    )
  }

  createPatient(patient: Patient) {
    return this.http.post(`${enviroment.apiBasePath}${this.patientPath}`, patient).pipe(
      tap(() => this.autorefresh$.next())
    )
  }
}
