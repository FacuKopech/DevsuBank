import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalFormComponent } from './mdoal-form.component';

describe('MdoalFormComponent', () => {
  let component: ModalFormComponent;
  let fixture: ComponentFixture<ModalFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
