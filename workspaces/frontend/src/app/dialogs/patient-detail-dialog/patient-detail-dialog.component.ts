import { NgTemplateOutlet } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  ReactiveFormsModule
} from '@angular/forms';
import { MessageService } from 'primeng/api';
import {
  DialogService,
  DynamicDialogConfig,
  DynamicDialogRef,
} from 'primeng/dynamicdialog';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { lastValueFrom } from 'rxjs';
import {
  Parameter,
  Patient,
  PatientService,
} from '../../services/patient.service';
import { cast } from '../../utils/cast';

@Component({
  selector: 'app-patient-detail-dialog',
  templateUrl: './patient-detail-dialog.component.html',
  styleUrl: './patient-detail-dialog.component.scss',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgTemplateOutlet,
    InputTextModule,
    TableModule,
  ],
})
export class PatientDetailDialogComponent {
  private readonly fb = inject(FormBuilder);
  private readonly patientService = inject(PatientService);
  private readonly toastService = inject(MessageService);

  readonly dialogRef = inject(DynamicDialogRef);
  readonly patient: Patient = inject(DynamicDialogConfig).data;

  readonly cast = cast<{ label: string; control: FormControl }>;
  readonly castParameter = cast<Parameter>;

  readonly form = this.fb.group({
    familyName: this.patient.familyName,
    givenName: this.patient.givenName,
    sex: this.patient.sex,
  });

  loading = false;

  static async open(dialogService: DialogService, data: Patient) {
    return await lastValueFrom(
      dialogService.open(PatientDetailDialogComponent, {
        appendTo: 'body',
        closable: true,
        data,
      }).onClose
    );
  }

  async submit() {
    if (!this.form.value) {
      return;
    }

    const patient: Patient = {
      ...this.patient,
      givenName: this.form.value.givenName ?? '',
      familyName: this.form.value.familyName ?? '',
      sex: this.form.value.sex ?? '',
    };

    try {
      this.loading = true;
      await lastValueFrom(this.patientService.updatePatient(patient));
      this.toastService.add({
        severity: 'success',
        summary: 'Successo',
        closable: true,
      });
    } catch {
      this.toastService.add({
        severity: 'error',
        summary: 'Errore',
        closable: true,
      });
    } finally {
      this.loading = false;
      this.dialogRef.close();
    }
  }
}
