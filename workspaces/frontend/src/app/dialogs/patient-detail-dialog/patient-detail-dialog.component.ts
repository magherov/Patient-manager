import { JsonPipe, NgTemplateOutlet } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
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
  PatientExtend,
  PatientService,
} from '../../services/patient.service';
import { cast } from '../../utils/cast';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';

const makeParameterForm = (fb: FormBuilder, parameter?: Parameter, isUpdate = false) => {
  return fb.nonNullable.group({
    id: [{ value: parameter?.id, disabled: true }],
    name: [{ value: parameter?.name, disabled: isUpdate }],
    value: [{ value: parameter?.value, disabled: isUpdate }],
    alarm: [{ value: parameter?.alarm, disabled: isUpdate }],
  });
}

@Component({
  selector: 'app-patient-detail-dialog',
  templateUrl: './patient-detail-dialog.component.html',
  styleUrl: './patient-detail-dialog.component.scss',
  standalone: true,
  imports: [
    CalendarModule,
    CheckboxModule,
    InputTextModule,
    NgTemplateOutlet,
    JsonPipe,
    ReactiveFormsModule,
    TableModule,
  ],
})
export class PatientDetailDialogComponent {
  private readonly fb = inject(FormBuilder);
  private readonly patientService = inject(PatientService);
  private readonly toastService = inject(MessageService);

  readonly dialogRef = inject(DynamicDialogRef);
  readonly data: PatientExtend = inject(DynamicDialogConfig).data;

  readonly cast = cast<{ label: string; control: FormControl }>;
  readonly castParameter = cast<ReturnType<typeof makeParameterForm>>;

  readonly form = this.fb.group({
    familyName: this.data.patient?.familyName,
    givenName: this.data.patient?.givenName,
    sex: this.data.patient?.sex,
    birthDate: new Date(this.data.patient?.birthDate?.toString() ?? Date.now()),
    parameters: this.fb.nonNullable.array((this.data.patient?.parameters ?? []).map((param) => makeParameterForm(this.fb, param, this.data.action === 'update'))),
  });

  loading = false;

  static async open(dialogService: DialogService, data: PatientExtend) {
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
      id: this.data.patient?.id ?? undefined,
      parameters: this.form.controls.parameters.getRawValue() ?? this.data.patient?.parameters,
      birthDate: this.form.value.birthDate ?? this.data.patient?.birthDate,
      givenName: this.form.value.givenName ?? this.data.patient?.givenName,
      familyName: this.form.value.familyName ?? this.data.patient?.familyName,
      sex: this.form.value.sex ?? this.data.patient?.sex,
    };

    try {
      this.loading = true;

      if (this.data.action === 'create') {
        await lastValueFrom(this.patientService.createPatient(patient));
      } else {
        await lastValueFrom(this.patientService.updatePatient(patient));
      }
      
      this.toastService.add({
        severity: 'success',
        summary: 'Salvataggio completato con successo',
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

  addParameter() {
    this.form.controls.parameters.controls.push(makeParameterForm(this.fb))
  }

  removeParameter(index: number, p: any) {
    console.log(p)
    this.form.controls.parameters.removeAt(index);
  }
}
