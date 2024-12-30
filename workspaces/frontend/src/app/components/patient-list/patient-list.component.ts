import { AsyncPipe, DatePipe, JsonPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { DialogService } from 'primeng/dynamicdialog';
import { TableModule } from 'primeng/table';
import {
  combineLatest,
  debounceTime,
  distinctUntilChanged,
  filter,
  lastValueFrom,
  map,
  startWith,
  switchMap,
} from 'rxjs';
import { PatientDetailDialogComponent } from '../../dialogs/patient-detail-dialog/patient-detail-dialog.component';
import {
  Parameter,
  Patient,
  PatientService,
} from '../../services/patient.service';
import { cast } from '../../utils/cast';
import { AuthService } from '../../services/auth.service';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';

const showAllarm = (parameter: Array<Parameter>) => parameter.at(-1)?.alarm;

@Component({
  selector: 'app-patient-list',
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.scss',
  standalone: true,
  imports: [
    AsyncPipe,
    TableModule,
    InputTextModule,
    ReactiveFormsModule,
    ConfirmDialogModule,
    DatePipe,
  ],
})
export default class PatientListComponent {
  private readonly patientService = inject(PatientService);
  private readonly dialogService = inject(DialogService);
  private readonly confirmationService = inject(ConfirmationService);

  readonly searchFamilyname = new FormControl('');
  readonly searchGivenname = new FormControl('');
  readonly sortField = new FormControl<string | null>(null);
  readonly sortDirection = new FormControl<'asc' | 'desc' | null>(null);
  readonly authService = inject(AuthService);

  readonly patients$ = combineLatest([
    this.searchFamilyname.valueChanges.pipe(
      startWith(''),
      debounceTime(1000),
      distinctUntilChanged()
    ),
    this.searchGivenname.valueChanges.pipe(
      startWith(''),
      debounceTime(1000),
      distinctUntilChanged()
    ),
    this.sortField.valueChanges.pipe(
      startWith(null),
      distinctUntilChanged()
    ),
    this.sortDirection.valueChanges.pipe(
      startWith(null),
      distinctUntilChanged()
    ),
    this.patientService.autorefresh$,
  ]).pipe(
    switchMap(([familyName, givenName, sortBy, sortDirection]) =>
      this.patientService.getPatientList({
        familyName: familyName ?? '',
        givenName: givenName ?? '',
        SortBy: sortBy,
        SortDirection: sortDirection,
      })
    ),
  );

  onSort(event: { field: string; order: 1 | -1 }) {
    this.sortField.setValue(event.field);
    this.sortDirection.setValue(event.order === 1 ? 'asc' : 'desc');
  }

  readonly currentUser = this.authService.getUser();
  readonly cast = cast<Patient>;
  readonly showAllarm = showAllarm;

  async openDetail(patient: Patient) {
    await PatientDetailDialogComponent.open(this.dialogService, { patient, action: 'update' });
  }

  async addPatient() {
    await PatientDetailDialogComponent.open(this.dialogService, { action: 'create' });
  }

  async deletePatient(id: number) {
    this.confirmationService.confirm({
      message: 'Sei sicuro di voler cancellare questo paziente ?',
      header: 'Conferma eliminazione',
      accept: async () => await lastValueFrom(this.patientService.deletePatient(id)),
      reject: () => { return }
    })
  }
}
