import { Component, inject } from '@angular/core';
import { TableModule } from 'primeng/table';
import { Parameter, Patient, PatientService } from '../../services/patient.service';
import { AsyncPipe, DatePipe, JsonPipe } from '@angular/common';
import { cast } from '../../utils/cast';
import { PatientDetailDialogComponent } from '../../dialogs/patient-detail-dialog/patient-detail-dialog.component';
import { DialogService } from 'primeng/dynamicdialog';
import { delay, switchMap } from 'rxjs';

const showAllarm = (parameter: Array<Parameter>) => parameter.at(-1)?.alarm;

@Component({
  selector: 'app-patient-list',
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.scss',
  standalone: true,
  imports: [AsyncPipe, TableModule, DatePipe],
})
export default class PatientListComponent {
  private readonly patientService = inject(PatientService);
  private readonly dialogService = inject(DialogService);

  readonly cast = cast<Patient>;
  readonly showAllarm = showAllarm;

  readonly patients$ = this.patientService.autorefresh$.pipe(
    switchMap(() => this.patientService.getPatientList()),
  );

  async openDetail(data: Patient) {
    await PatientDetailDialogComponent.open(this.dialogService, data);
  }
}
