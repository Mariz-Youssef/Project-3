import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Card } from "../../components/common/Card";
import { FormField, TextInput } from "../../components/common/FormField";
import { Button } from "../../components/common/Button";

export function FindAppointmentPage() {
  const navigate = useNavigate();
  const [id, setId] = useState("");
  const [error, setError] = useState(null);

  function handleSubmit(e) {
    e.preventDefault();
    const parsed = Number(id);
    if (!parsed) {
      setError("Enter a valid appointment ID.");
      return;
    }
    navigate(`/appointments/${parsed}`);
  }

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <p className="page-eyebrow">Scheduling</p>
          <h1 className="page-title">Look up an appointment</h1>
          <p className="page-subtitle">
            Enter an appointment ID to view its details and available actions.
          </p>
        </div>
      </div>

      <Card style={{ maxWidth: 360 }}>
        <form className="stack-vertical" onSubmit={handleSubmit}>
          <FormField label="Appointment ID" htmlFor="find-id" error={error}>
            <TextInput
              id="find-id"
              type="number"
              min="1"
              value={id}
              onChange={(e) => setId(e.target.value)}
              placeholder="e.g. 42"
              autoFocus
            />
          </FormField>
          <div className="form-actions">
            <Button type="submit">View appointment</Button>
          </div>
        </form>
      </Card>
    </div>
  );
}
