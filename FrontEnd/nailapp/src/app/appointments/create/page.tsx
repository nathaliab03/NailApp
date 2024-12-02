"use client";
import React, { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import { Listbox, ListboxButton, ListboxOption, ListboxOptions } from "@headlessui/react";
import Link from "next/link";

interface NailStylist {
  id: number;
  name: string;
  isAvailable: boolean;
}

interface Client {
  id: number;
  name: string;
}

export default function CreateAppointment() {
  const router = useRouter();
  const [role, setRole] = useState<string | null>("");
  const [nailStylistId, setNailStylistId] = useState<number | null>(null);
  const [clients, setClients] = useState<Client[]>([]);
  const [client, setClient] = useState<number | null>(null);
  const [nailStylists, setNailStylists] = useState<NailStylist[]>([]);
  const [error, setError] = useState<string | null>("");
  const [availableDates, setAvailableDates] = useState<Record<string, string[]>>({});
  const [availableHours, setAvailableHours] = useState<string[]>([]);
  const [date, setDate] = useState<string>("");
  const [hour, setHour] = useState<string>("");

  useEffect(() => {
    const userRole = sessionStorage.getItem("role");
    setRole(userRole);

    if (userRole === "Admin") {
      fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/users`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: sessionStorage.getItem("token") || "",
        },
      })
        .then((response) => response.json())
        .then((data) => {
          setClients(data.content);
        });
    } else if (userRole === "User") {
      const user = JSON.parse(sessionStorage.getItem("user") || "{}");
      setClient(user.id);
    }

    fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/nailstylist`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: sessionStorage.getItem("token") || "",
      },
    })
      .then((response) => response.json())
      .then((data) => {
        setNailStylists(data.content);
      });
  }, []);

  const fetchAvailableDates = (id: string) => {
    if (!id) return;
  
    fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/appointment/availableDates?stylistId=${id}`)
      .then((response) => response.json())
      .then((data) => {
        if (data.isSuccess && data.content) {
          const dateData: Record<string, string[]> = data.content;
          setAvailableDates(dateData);
        } else {
          setAvailableDates({});
        }
      })
      .catch(() => setAvailableDates({}));
  };
  

  const fetchAvailableHours = (selectedDate: string) => {
    const selectedHours = availableDates[selectedDate];
  
    if (selectedHours) {
      setAvailableHours(selectedHours);
    } else {
      setAvailableHours([]);
    }
  };
  

  const addAppointment = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!date || !hour || !nailStylistId || !client) {
      setError("Todos os campos devem ser preenchidos.");
      return;
    }

    const formData = {
      date: new Date(date).toISOString(),
      nailStylistId,
      clientId: client,
      time: hour.includes(":") ? hour : `${hour}:00`,
    };

    const add = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/appointment/create`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: sessionStorage.getItem("token") || "",
      },
      body: JSON.stringify(formData),
    });

    const content = await add.json();

    if (content.isSuccess) {
      router.push("/home");
    } else {
      setError(content.error || "Erro ao agendar o horário.");
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate().toString().padStart(2, '0')}/${(date.getMonth() + 1).toString().padStart(2, '0')}/${date.getFullYear()}`;
  };

  return (
    <div className="flex items-center justify-center h-screen">

      
      <form className="container w-96" onSubmit={addAppointment}>
        <span className="font-bold text-white py-2 block text-2xl text-center">Agendar Horário</span>

        {/* NailStylist Listbox */}
        <div className="flex items-center relative w-full py-2 space-x-2">
          <span className="text-sm text-right font-medium w-full">Nail Stylist:</span>
          <Listbox value={nailStylistId ?? null} onChange={(id) => { setNailStylistId(id); fetchAvailableDates(id); }}>
            <div className="relative">
              <ListboxButton className="w-72 text-left input-custom pt-3.5 rounded-sm">
                {nailStylists.find((stylist) => stylist.id === nailStylistId)?.name || "Selecione"}
              </ListboxButton>
              <ListboxOptions className="absolute w-full mt-1 bg-white shadow-lg rounded-sm z-10">
                {nailStylists.map((stylist) => (
                  <ListboxOption
                    key={stylist.id}
                    value={stylist.id}
                    className="data-[focus]:bg-blue-100 w-full input-custom pl-3 py-2 rounded-sm focus:outline-none"
                  >
                    {stylist.name}
                  </ListboxOption>
                ))}
              </ListboxOptions>
            </div>
          </Listbox>
        </div>

        {/* Client Listbox */}
        <div className="flex items-center relative w-full py-2 space-x-2">
          <span className="text-sm text-right font-medium w-full">Client:</span>
          {role === "User" ? (
            <input
              type="text"
              className="w-full input-custom pl-3 pt-3.5 rounded-sm focus:outline-none"
              value={clients.find((c) => c.id === client)?.name || ""}
              readOnly
            />
          ) : (
            <Listbox value={client} onChange={setClient}>
              <div className="relative">
                <ListboxButton className="w-72 text-left input-agenda pt-3.5 rounded-sm">
                  {clients.find((c) => c.id === client)?.name || "Selecione um Cliente"}
                </ListboxButton>
                <ListboxOptions className="absolute w-full mt-1 bg-white shadow-lg rounded-sm z-10">
                  {clients.map((client) => (
                    <ListboxOption
                      key={client.id}
                      value={client.id}
                      className="input-agenda input-agenda-option pl-3 py-2 rounded-sm focus:outline-none"
                    >
                      {client.name}
                    </ListboxOption>
                  ))}
                </ListboxOptions>
              </div>
            </Listbox>
          )}
        </div>

        {/* Data Select */}
<div className="flex items-center relative w-full py-2 space-x-2">
  <span className="text-sm text-right font-medium w-32">Data:</span>
  <select
    className="w-full input-custom pl-3 pt-3.5 rounded-sm focus:outline-none"
    value={date}
    onChange={(e) => { setDate(e.target.value); fetchAvailableHours(e.target.value); }}
  >
    <option value="">Selecione uma data</option>
    {Object.keys(availableDates).length > 0 ? (
      Object.keys(availableDates).map((dateOption, index) => {
        const formattedDate = formatDate(dateOption); // Formata a data
        return (
          <option key={index} value={dateOption}>
            {formattedDate}
          </option>
        );
      })
    ) : (
      <option value="">Sem datas disponíveis</option>
    )}
  </select>
</div>

        {/* Hora Select */}
        <div className="flex items-center relative w-full py-2 space-x-2">
          <span className="text-sm text-right font-medium w-32">Hora:</span>
          <select
            className="w-full input-custom pl-3 pt-3.5 rounded-sm focus:outline-none"
            value={hour}
            onChange={(e) => setHour(e.target.value)}
            disabled={!date}
          >
            <option value="">Selecione uma hora</option>
            {availableHours.map((hourOption, index) => (
              <option key={index} value={hourOption}>
                {hourOption}
              </option>
            ))}
          </select>
        </div>

        {/* Error */}
        {error && <div className="text-red-500 text-center py-2">{error}</div>}

        {/* Submit Button */}
        <div className="w-full flex justify-center py-3 mt-5">
          <button type="submit" className="w-full p-2 border-gray-200 border-[1px] rounded-sm btn-primary uppercase">Agendar</button>
        </div>
        <div className="w-full flex justify-center py-3">
          <Link type="submit" href="../home">Voltar</Link>
        </div>
      </form>
    </div>
  );
}